FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DLS_Catalog_Service_3.0.csproj", "./"]
RUN dotnet restore "DLS_Catalog_Service_3.0.csproj"
COPY . .
RUN dotnet build "DLS_Catalog_Service_3.0.csproj" -c Release -o /app/build

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
COPY --from=build /app/build .
EXPOSE 80
EXPOSE 443
ENTRYPOINT ["dotnet", "DLS_Catalog_Service_3.0.dll"]
