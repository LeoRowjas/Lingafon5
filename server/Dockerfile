# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
# Copy csproj files and restore dependencies
COPY Lingafon.Core/Lingafon.Core.csproj Lingafon.Core/
COPY Lingafon.Application/Lingafon.Application.csproj Lingafon.Application/
COPY Lingafon.Infrastructure/Lingafon.Infrastructure.csproj Lingafon.Infrastructure/
COPY Lingafon.API/Lingafon.API.csproj Lingafon.API/
RUN dotnet restore Lingafon.API/Lingafon.API.csproj
# Copy everything else and build
COPY . .
RUN dotnet publish Lingafon.API/Lingafon.API.csproj -c Release -o /app/publish
# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
# Set environment variables
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5000
ENTRYPOINT ["dotnet", "Lingafon.API.dll"]
