#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Authenticate.Api.csproj", "Authenticate/Authenticate.Api/"]
RUN dotnet restore "Authenticate/Authenticate.Api/Authenticate.Api.csproj"
COPY . "Authenticate/Authenticate.Api/"
WORKDIR "/src/Authenticate/Authenticate.Api"
RUN dotnet build "Authenticate.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Authenticate.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authenticate.Api.dll"]