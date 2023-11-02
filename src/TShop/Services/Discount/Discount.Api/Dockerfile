#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["Services/Discount/Discount.Api/Discounts.Api.csproj", "Services/Discount/Discount.Api/"]
RUN dotnet restore "Services/Discount/Discount.Api/Discounts.Api.csproj"

COPY ["Services/Discount/Discount.Api", "Services/Discount/Discount.Api/"]

WORKDIR "/src/Services/Discount/Discount.Api"
RUN dotnet build "Discounts.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Discounts.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Discounts.Api.dll"]