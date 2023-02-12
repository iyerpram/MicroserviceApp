#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/api/MicroserviceApp.Api.Orders/MicroserviceApp.Api.Orders.csproj", "src/api/MicroserviceApp.Api.Orders/"]
RUN dotnet restore "src/api/MicroserviceApp.Api.Orders/MicroserviceApp.Api.Orders.csproj"
COPY . .
WORKDIR "/src/src/api/MicroserviceApp.Api.Orders"
RUN dotnet build "MicroserviceApp.Api.Orders.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceApp.Api.Orders.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceApp.Api.Orders.dll"]