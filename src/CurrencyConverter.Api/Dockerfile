# ------------- BUILD STAGE -------------
# Use appropriate SDK version (adjust if your project targets .NET 6, 7, 8, etc.)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the .csproj and restore (so that restore is cached)
COPY ["CurrencyConverter.Api/CurrencyConverter.Api.csproj", "CurrencyConverter.Api/"]
RUN dotnet restore "CurrencyConverter.Api/CurrencyConverter.Api.csproj"

# Copy all source files
COPY . .

WORKDIR "/src/CurrencyConverter.Api"
# Publish in Release mode
RUN dotnet publish "CurrencyConverter.Api.csproj" -c Release -o /app/publish

# ------------- RUNTIME STAGE -------------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copy published output from build stage
COPY --from=build /app/publish .

# Configure environment (e.g. listen on port 80)
ENV ASPNETCORE_URLS="http://+:80"
ENV ASPNETCORE_ENVIRONMENT=Development
ENV DOTNET_RUNNING_IN_CONTAINER=true
# (If your app expects other environment variables, set them here or via docker-compose)

EXPOSE 8080

ENTRYPOINT ["dotnet", "CurrencyConverter.Api.dll"]