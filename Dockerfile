# Use official .NET SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# Copy the project files to the container
COPY ["MonitoringApp.csproj", "./"]
COPY ./Services/ ./Services/
COPY ./Utilities/ ./Utilities/
COPY ./Program.cs ./Program.cs
COPY ./appsettings.json ./appsettings.json

# Restore any dependencies
RUN dotnet restore

# Build the project
RUN dotnet build -c Release -o /app/build

# Publish the project
RUN dotnet publish -c Release -o /app/publish

# Use official .NET runtime image to run the project
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Define the entry point for the container
ENTRYPOINT ["dotnet", "MonitoringApp.dll"]
