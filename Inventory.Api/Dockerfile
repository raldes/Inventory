#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Inventory.Api/Inventory.Api.csproj", "Inventory.Api/"]
COPY ["Inventory.App/Inventory.App.csproj", "Inventory.App/"]
COPY ["Inventory.Infra/Inventory.Infra.csproj", "Inventory.Infra/"]
COPY ["Inventory.Domain/Inventory.Domain.csproj", "Inventory.Domain/"]
RUN dotnet restore "Inventory.Api/Inventory.Api.csproj"
COPY . .
WORKDIR "/src/Inventory.Api"
RUN dotnet build "Inventory.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inventory.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inventory.Api.dll"]