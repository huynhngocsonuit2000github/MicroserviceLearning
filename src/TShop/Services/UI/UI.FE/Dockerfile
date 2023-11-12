#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["Services/UI/UI.FE/UI.FE.csproj", "Services/UI/UI.FE/"]
RUN dotnet restore "Services/UI/UI.FE/UI.FE.csproj"

COPY ["Services/UI/UI.FE", "Services/UI/UI.FE/"]

WORKDIR "/src/Services/UI/UI.FE"
RUN dotnet build "UI.FE.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "UI.FE.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "UI.FE.dll"]