#Obtain Microsoft Docker Image SDK, also can get runtime
FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
#Copy *.csproj files to the container and restore nuget packages
COPY ["Execute/Console Test/ConsoleTest/*.csproj", "./Execute/Console Test/ConsoleTest/"]
COPY ["Core/Gruggbot.Core/*.csproj", "./Core/Gruggbot.Core/"]
RUN dotnet restore "./Execute/Console Test/ConsoleTest/ConsoleTest.csproj"

#copy all other files to container
COPY . .
WORKDIR /src/Execute/Console Test/ConsoleTest

#Build the Project
RUN dotnet build ConsoleTest.csproj -c Release -o /app
#publish the Project
RUN dotnet publish ConsoleTest.csproj -c Release -o /app
WORKDIR /app
ENTRYPOINT [ "dotnet", "ConsoleTest.dll" ]