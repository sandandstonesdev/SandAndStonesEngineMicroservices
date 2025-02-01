FROM mcr.microsoft.com/dotnet/sdk:9.0 AS base
WORKDIR /app

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["Directory.Packages.props","/"]
COPY ["SandAndStones.Infrastructure/SandAndStones.Infrastructure.csproj", "SandAndStones.Infrastructure/"]
COPY ["SandAndStones.Domain/SandAndStones.Domain.csproj", "SandAndStones.Domain/"]
COPY ["SandAndStones.App/SandAndStones.App.csproj", "SandAndStones.App/"]
COPY ["SandAndStones.EventLog.Api/SandAndStones.EventLog.Api.csproj", "SandAndStones.EventLog.Api/"]
RUN dotnet restore "SandAndStones.EventLog.Api/SandAndStones.EventLog.Api.csproj"
COPY . .
WORKDIR "/src/SandAndStones.EventLog.Api"
RUN dotnet build "SandAndStones.EventLog.Api.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "SandAndStones.EventLog.Api.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "SandAndStones.EventLog.Api.dll"]