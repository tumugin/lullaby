# see: https://hub.docker.com/_/microsoft-dotnet-sdk/
FROM mcr.microsoft.com/dotnet/sdk:7.0.102 AS build-env

WORKDIR /App

# Copy csproj
COPY Lullaby/Lullaby.csproj ./
# Restore as distinct layers
RUN dotnet restore
# Copy everything else and build
COPY Lullaby ./
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
# see: https://hub.docker.com/_/microsoft-dotnet-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:7.0.2
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "Lullaby.dll"]
