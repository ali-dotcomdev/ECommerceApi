FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["ECommerceApi.API/ECommerceApi.API.csproj", "ECommerceApi.API/"]
COPY ["ECommerceApi.Application/ECommerceApi.Application.csproj", "ECommerceApi.Application/"]
COPY ["ECommerceApi.Domain/ECommerceApi.Domain.csproj", "ECommerceApi.Domain/"]
COPY ["ECommerceApi.Infrastructure/ECommerceApi.Infrastructure.csproj", "ECommerceApi.Infrastructure/"]

RUN dotnet restore "ECommerceApi.API/ECommerceApi.API.csproj"

COPY . .

WORKDIR "/src/ECommerceApi.API"
RUN dotnet publish "ECommerceApi.API.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ECommerceApi.API.dll"]