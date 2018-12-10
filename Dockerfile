FROM microsoft/dotnet:2.1-runtime as base
WORKDIR /app

#Obtain Microsoft Docker Image SDK, also can get runtime
FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
#Copy *.csproj files to the container and restore nuget packages
COPY ["GruggbotEntry/*.csproj", "./GruggbotEntry/"]
COPY ["Core/Gruggbot.Core/*.csproj", "./Core/Gruggbot.Core/"]
RUN dotnet restore "./GruggbotEntry/GruggbotEntry.csproj"
RUN dotnet restore "./Core/Gruggbot.Core/Gruggbot.Core.csproj"

#copy all other files to container
COPY . .
WORKDIR /src/GruggbotEntry

#Build the Project
RUN dotnet build GruggbotEntry.csproj -c Release -o /app
#publish the Project
FROM build AS publish
RUN dotnet publish GruggbotEntry.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT [ "dotnet", "GruggbotEntry.dll" ]