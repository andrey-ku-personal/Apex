FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080


FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

COPY ["backend/Apex/src/Apex.Invest.Api/Apex.Invest.Api.csproj", "backend/Apex/src/Apex.Invest.Api/"]
COPY ["backend/Apex/src/Apex.Invest.Features/Apex.Invest.Features.csproj", "backend/Apex/src/Apex.Invest.Features/"]
COPY ["backend/Apex/src/Apex.Invest.Domain/Apex.Invest.Domain.csproj", "backend/Apex/src/Apex.Invest.Domain/"]
COPY ["backend/Apex/src/Apex.Invest.Persistance/Apex.Invest.Persistance.csproj", "backend/Apex/src/Apex.Invest.Persistance/"]
COPY ["backend/Apex/src/Apex.Invest/Apex.Shared.Core/Apex.Shared.Core.csproj", "backend/Apex/src/Apex.Invest/Apex.Shared.Core/"]

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet restore "backend/Apex/src/Apex.Invest.Api/Apex.Invest.Api.csproj"

COPY backend/Apex/ backend/Apex/

RUN --mount=type=cache,id=nuget,target=/root/.nuget/packages \
    dotnet publish "backend/Apex/src/Apex.Invest.Api/Apex.Invest.Api.csproj" \
    -c $BUILD_CONFIGURATION -o /app/publish \
    /p:UseAppHost=false --no-restore


FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Apex.Invest.Api.dll"]
