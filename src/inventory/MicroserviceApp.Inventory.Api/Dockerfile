FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/inventory/MicroserviceApp.Inventory.Api/MicroserviceApp.Inventory.Api.csproj", "src/inventory/MicroserviceApp.Inventory.Api/"]
RUN dotnet restore "src/inventory/MicroserviceApp.Inventory.Api/MicroserviceApp.Inventory.Api.csproj"
COPY . .
WORKDIR "/src/src/inventory/MicroserviceApp.Inventory.Api"
RUN dotnet build "MicroserviceApp.Inventory.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceApp.Inventory.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceApp.Inventory.Api.dll"]
