#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["RestaurantSimulation.Api/RestaurantSimulation.Api.csproj", "RestaurantSimulation.Api/"]
COPY ["RestaurantSimulation.Contracts/RestaurantSimulation.Contracts.csproj", "RestaurantSimulation.Contracts/"]
COPY ["RestaurantSimulation.Infrastructure/RestaurantSimulation.Infrastructure.csproj", "RestaurantSimulation.Infrastructure/"]
COPY ["RestaurantSimulation.Application/RestaurantSimulation.Application.csproj", "RestaurantSimulation.Application/"]
COPY ["RestaurantSimulation.Domain/RestaurantSimulation.Domain.csproj", "RestaurantSimulation.Domain/"]
RUN dotnet restore "RestaurantSimulation.Api/RestaurantSimulation.Api.csproj"
COPY . .
WORKDIR "/src/RestaurantSimulation.Api"
RUN dotnet build "RestaurantSimulation.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RestaurantSimulation.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RestaurantSimulation.Api.dll"]