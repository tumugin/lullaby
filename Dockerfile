# see: https://hub.docker.com/_/microsoft-dotnet-sdk/
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env

WORKDIR /App

# Copy csproj
COPY *.sln .
COPY Lullaby/*.csproj ./Lullaby/
COPY Lullaby.Database/*.csproj ./Lullaby.Database/
COPY Lullaby.Tests/*.csproj ./Lullaby.Tests/

# Restore as distinct layers
RUN dotnet restore

# Copy everything else
COPY . ./
# Build and publish a release
WORKDIR Lullaby
RUN dotnet publish -c Release -o out

# Build runtime image
# see: https://hub.docker.com/_/microsoft-dotnet-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build-env /App/Lullaby/out .
ENTRYPOINT ["dotnet", "Lullaby.dll"]
