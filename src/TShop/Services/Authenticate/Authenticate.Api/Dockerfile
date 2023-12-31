#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 443
ENV ASPNETCORE_URLS=https://+:443
COPY "certs/tls.pfx" . 

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["Services/Authenticate/Authenticate.Api/Authenticate.Api.csproj", "Services/Authenticate/Authenticate.Api/"]
RUN dotnet restore "Services/Authenticate/Authenticate.Api/Authenticate.Api.csproj"

COPY ["Services/Authenticate/Authenticate.Api", "Services/Authenticate/Authenticate.Api/"]

WORKDIR "/src/Services/Authenticate/Authenticate.Api"
RUN dotnet build "Authenticate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authenticate.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authenticate.Api.dll"]