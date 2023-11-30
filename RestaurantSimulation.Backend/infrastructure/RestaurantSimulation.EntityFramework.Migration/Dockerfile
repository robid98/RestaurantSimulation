#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["infrastructure/RestaurantSimulation.EntityFramework.Migration/RestaurantSimulation.EntityFramework.Migration.csproj", "infrastructure/RestaurantSimulation.EntityFramework.Migration/"]
COPY ["src/RestaurantSimulation.Infrastructure/RestaurantSimulation.Infrastructure.csproj", "src/RestaurantSimulation.Infrastructure/"]
COPY ["src/RestaurantSimulation.Application/RestaurantSimulation.Application.csproj", "src/RestaurantSimulation.Application/"]
COPY ["src/RestaurantSimulation.Domain/RestaurantSimulation.Domain.csproj", "src/RestaurantSimulation.Domain/"]
RUN dotnet restore "infrastructure/RestaurantSimulation.EntityFramework.Migration/RestaurantSimulation.EntityFramework.Migration.csproj"
COPY . .
WORKDIR "/src/infrastructure/RestaurantSimulation.EntityFramework.Migration"
RUN dotnet build "RestaurantSimulation.EntityFramework.Migration.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantSimulation.EntityFramework.Migration.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantSimulation.EntityFramework.Migration.dll"]