# Base image
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# Install MongoDB client
RUN apt-get update \
    && apt-get install -y gnupg curl \
    && curl -fsSL https://www.mongodb.org/static/pgp/server-4.4.asc | apt-key add - \
    && echo "deb [ arch=amd64,arm64 ] https://repo.mongodb.org/apt/ubuntu focal/mongodb-org/4.4 multiverse" | tee /etc/apt/sources.list.d/mongodb-org-4.4.list \
    && apt-get update \
    && apt-get install -y mongodb-org-shell \
    && rm -rf /var/lib/apt/lists/*

# Build image
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY ["DLS_Catalog_Service_3.0/DLS_Catalog_Service_3.0.csproj", "DLS_Catalog_Service_3.0/"]

# Restore .NET packages
RUN dotnet restore "DLS_Catalog_Service_3.0/DLS_Catalog_Service_3.0.csproj"

# Copy everything else and build
COPY . .
WORKDIR "/src/DLS_Catalog_Service_3.0"
RUN dotnet build "DLS_Catalog_Service_3.0.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DLS_Catalog_Service_3.0.csproj" -c Release -o /app/publish

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY DLS_Catalog_Service_3.0/entrypoint.sh /app/entrypoint.sh
COPY DLS_Catalog_Service_3.0/wait-for-it.sh /app/wait-for-it.sh
RUN chmod +x /app/entrypoint.sh
RUN chmod +x /app/wait-for-it.sh
ENTRYPOINT ["/app/entrypoint.sh"]
