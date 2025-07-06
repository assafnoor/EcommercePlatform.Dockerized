using Prometheus;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

builder.Services.AddLogging(); // Optional, already included usually

var app = builder.Build();

app.MapGet("/health", () => "Gateway is healthy ✅");

// ✅ Monitoring Middleware (Prometheus)
app.UseMetricServer();       // exposes /metrics
app.UseHttpMetrics();

// ✅ Logging Middleware
app.Use(async (context, next) =>
{
    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();
    logger.LogInformation("Incoming request: {method} {url}", context.Request.Method, context.Request.Path);

    await next();

    logger.LogInformation("Response: {statusCode}", context.Response.StatusCode);
});

// ✅ Reverse Proxy
app.MapReverseProxy();

app.Run();