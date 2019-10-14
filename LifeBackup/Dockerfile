FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["LifeBackup/LifeBackup.csproj", "LifeBackup/"]
RUN dotnet restore "LifeBackup/LifeBackup.csproj"
COPY . .
WORKDIR "/src/LifeBackup"
RUN dotnet build "LifeBackup.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "LifeBackup.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LifeBackup.dll"]