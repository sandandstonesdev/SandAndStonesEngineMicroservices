FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /app

ENV ASPNETCORE_ENVIRONMENT Local

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["SandAndStones.Infrastructure/SandAndStones.Infrastructure.csproj", "SandAndStones.Infrastructure/"]
COPY ["SandAndStones.Domain/SandAndStones.Domain.csproj", "SandAndStones.Domain/"]
COPY ["SandAndStones.App/SandAndStones.App.csproj", "SandAndStones.App/"]
COPY ["SandAndStones.Gateway.Api/SandAndStones.Gateway.Api.csproj", "SandAndStones.Gateway.Api/"]
RUN dotnet restore "SandAndStones.Gateway.Api/SandAndStones.Gateway.Api.csproj"
COPY . .
WORKDIR "/src/SandAndStones.Gateway.Api"
RUN dotnet build "SandAndStones.Gateway.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SandAndStones.Gateway.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SandAndStones.Gateway.Api.dll"]