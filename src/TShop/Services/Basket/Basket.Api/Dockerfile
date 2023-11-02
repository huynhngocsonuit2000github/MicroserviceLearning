#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["Services/Basket/Basket.Api/Basket.Api.csproj", "Services/Basket/Basket.Api/"]
COPY ["Services/Common/Common/Common.csproj", "Services/Common/Common/"]
RUN dotnet restore "Services/Basket/Basket.Api/Basket.Api.csproj"

COPY ["Services/Basket/Basket.Api", "Services/Basket/Basket.Api/"]
COPY ["Services/Common/Common", "Services/Basket/Basket.Api/"]

WORKDIR "/src/Services/Basket/Basket.Api"
RUN dotnet build "Basket.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Basket.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Basket.Api.dll"]