#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["Services/Ordering/Ordering.Api/Ordering.Api.csproj", "Services/Ordering/Ordering.Api/"]
COPY ["Services/Common/Common/Common.csproj", "Services/Common/Common/"]
RUN dotnet restore "Services/Ordering/Ordering.Api/Ordering.Api.csproj"

COPY ["Services/Ordering/Ordering.Api", "Services/Ordering/Ordering.Api/"]
COPY ["Services/Common/Common", "Services/Ordering/Ordering.Api/"]

WORKDIR "/src/Services/Ordering/Ordering.Api"
RUN dotnet build "Ordering.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.Api.dll"]