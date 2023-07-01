#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/RestaurantSimulation.Api/RestaurantSimulation.Api.csproj", "src/RestaurantSimulation.Api/"]
COPY ["src/RestaurantSimulation.Application/RestaurantSimulation.Application.csproj", "src/RestaurantSimulation.Application/"]
COPY ["src/RestaurantSimulation.Domain/RestaurantSimulation.Domain.csproj", "src/RestaurantSimulation.Domain/"]
COPY ["src/RestaurantSimulation.Contracts/RestaurantSimulation.Contracts.csproj", "src/RestaurantSimulation.Contracts/"]
COPY ["src/RestaurantSimulation.Infrastructure/RestaurantSimulation.Infrastructure.csproj", "src/RestaurantSimulation.Infrastructure/"]
RUN dotnet restore "src/RestaurantSimulation.Api/RestaurantSimulation.Api.csproj"
COPY . .
WORKDIR "/src/src/RestaurantSimulation.Api"
RUN dotnet build "RestaurantSimulation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantSimulation.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantSimulation.Api.dll"]