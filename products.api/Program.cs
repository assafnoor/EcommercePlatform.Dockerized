using Microsoft.EntityFrameworkCore;
using products.api.models;
using Scalar.AspNetCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddOpenApi();
    builder.Host.UseSerilog((context, services, configuration) =>
    {
        configuration
            .WriteTo.Console()
            .WriteTo.File("/app/logs/log-.txt", rollingInterval: RollingInterval.Day);
    });

    builder.Services.AddDbContext<ProductsDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
}

var app = builder.Build();
{
    app.MapOpenApi();
    app.MapScalarApiReference();
    app.UseHttpsRedirection();

    app.MapGet("/health", () => Results.Ok("Products API healthy"));
    app.MapGet("/api/products/health", () => Results.Ok("Products API healthy"));
    app.MapGet("/api/products", async (ProductsDbContext db) =>
    {
        var products = await db.Products.AsNoTracking().ToListAsync();
        return Results.Ok(products);
    });
    app.MapPost("/api/products", async (Product product, ProductsDbContext db) =>
    {
        db.Products.Add(product);
        await db.SaveChangesAsync();
        return Results.Created($"/api/products/{product.Id}", product);
    });

    using (var scope = app.Services.CreateScope())
    {
        var context = scope.ServiceProvider.GetRequiredService<ProductsDbContext>();
        context.Database.Migrate();
    }
    app.Run();
}