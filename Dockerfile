#Obtain Microsoft Docker Image SDK, also can get runtime
FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
#Copy *.csproj files to the container and restore nuget packages

#copy all other files to container
COPY . .

#Build the Project
ENTRYPOINT [ "dotnet", "GruggbotEntry.dll" ]