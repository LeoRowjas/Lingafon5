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

# Install system dependencies required for Whisper.net and general use
RUN apt-get update && apt-get install -y \
    libgomp1 \
    libopenblas0 \
    liblapack3 \
    libblas3 \
    libc6 \
    libgcc-s1 \
    ffmpeg \
    curl \
    && rm -rf /var/lib/apt/lists/*

COPY --from=build /app/publish .

# Copy pre-included whisper model from root
COPY ggml-tiny.bin ./

# Set environment variables
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production
ENV LD_LIBRARY_PATH=/app/runtimes/linux-arm64:$LD_LIBRARY_PATH

EXPOSE 5000
ENTRYPOINT ["dotnet", "Lingafon.API.dll"]
