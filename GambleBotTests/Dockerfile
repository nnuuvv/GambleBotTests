FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["GambleBotTests/GambleBotTests.csproj", "GambleBotTests/"]
RUN dotnet restore "GambleBotTests/GambleBotTests.csproj"
COPY . .
WORKDIR "/src/GambleBotTests"
RUN dotnet build "GambleBotTests.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GambleBotTests.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GambleBotTests.dll"]
