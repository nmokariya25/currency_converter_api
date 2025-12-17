# Currency Converter API

A robust, scalable, and maintainable **Currency Converter API** built with **C#** and **ASP.NET Core**. Designed for high performance, security, and resilience, this API fetches currency exchange rates, supports historical queries, and converts amounts between currencies.

---

## Features

- Fetch **latest exchange rates** for a specific base currency.
- Convert amounts between supported currencies (**excluding TRY, PLN, THB, MXN**).
- Retrieve **historical exchange rates** with pagination support.
- **Caching** for improved performance and reduced API calls.
- **Retry policies** with exponential backoff and **circui**

## Architecture & Design

- **Dependency Injection (DI)** for service abstractions.
- **Factory Pattern** for dynamic currency provider selection.
- Resilient API calls using **caching, retry policies, and circuit breakers**.
- **Structured logging** using Serilog (supports Seq or ELK stack).
- **Distributed tracing** with OpenTelemetry.
- Clean separation of concerns following **SOLID principles**.

## Getting Started

### Prerequisites

- .NET 8.0
- Visual Studio 2022 or VS Code

### Installation

1. Clone the repository:

```bash
[git clone https://github.com/nmokariya25/currency-converter-api.git](https://github.com/nmokariya25/currency_converter_api.git)

cd currency-converter-api

2. Restore dependencies:

dotnet restore

3. Build the project:

dotnet build

4. Run the API:

cd src

dotnet run --project ./CurrencyConverter.Api/CurrencyConverter.Api.csproj

Endpoints

## Endpoints

| Endpoint               | Method | Description                          |
|------------------------|--------|--------------------------------------|
| /api/rates/latest      | GET    | Fetch latest exchange rates           |
| /api/rates/convert     | POST   | Convert amount between currencies    |
| /api/rates/history     | GET    | Retrieve historical exchange rates   |
