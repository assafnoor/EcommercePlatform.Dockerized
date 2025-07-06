# EcommercePlatform.Dockerized

A professional, Dockerized microservices e-commerce platform built with .NET. This project demonstrates scalable, modular architecture using Docker Compose for orchestration.

## Features
- Microservices: basket, orders, products, and gateway APIs
- Fully containerized with Docker
- Centralized API gateway
- Shared library for common models and logic
- Easy local development and deployment

## Project Structure
```
EcommercePlatform.Dockerized.sln         # Solution file
basket.api/                             # Basket service
orders.api/                             # Orders service
products.api/                           # Products service
shared/                                 # Shared models and logic
gateway/                                # API Gateway
logs/                                   # Log files
docker-compose.yml                      # Docker Compose for orchestration
```

## Prerequisites
- [Docker](https://www.docker.com/get-started)
- [.NET 9 SDK](https://dotnet.microsoft.com/download) (for local development)

## Build and Run with Docker

1. **Clone the repository:**
   ```sh
   git clone <your-repo-url>
   cd EcommercePlatform.Dockerized
   ```
2. **Build and start all services:**
   ```sh
   docker-compose up --build
   ```
3. **Stop all services:**
   ```sh
   docker-compose down
   ```

All services will be available on their respective ports as defined in `docker-compose.yml`.

## Local Development (Optional)
You can also run and debug individual services using the .NET CLI:
```sh
# Example: Run products.api
 dotnet run --project products.api/products.api.csproj
```

## Contributing
Contributions are welcome! Please fork the repository and submit a pull request.

## License
This project is licensed under the MIT License.

