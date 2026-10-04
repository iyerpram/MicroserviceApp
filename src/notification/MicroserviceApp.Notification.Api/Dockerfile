FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/notification/MicroserviceApp.Notification.Api/MicroserviceApp.Notification.Api.csproj", "src/notification/MicroserviceApp.Notification.Api/"]
RUN dotnet restore "src/notification/MicroserviceApp.Notification.Api/MicroserviceApp.Notification.Api.csproj"
COPY . .
WORKDIR "/src/src/notification/MicroserviceApp.Notification.Api"
RUN dotnet build "MicroserviceApp.Notification.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MicroserviceApp.Notification.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MicroserviceApp.Notification.Api.dll"]
