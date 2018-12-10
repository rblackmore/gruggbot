FROM microsoft/dotnet:2.1-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY GruggbotEntry/GruggbotEntry.csproj GruggbotEntry/
COPY Core/Gruggbot.Core/Gruggbot.Core.csproj Core/Gruggbot.Core/
RUN dotnet restore GruggbotEntry/GruggbotEntry.csproj
COPY . .
WORKDIR /src/GruggbotEntry
RUN dotnet build GruggbotEntry.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish GruggbotEntry.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "GruggbotEntry.dll"]
