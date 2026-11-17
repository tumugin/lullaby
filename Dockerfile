# see: https://hub.docker.com/_/microsoft-dotnet-sdk/
FROM mcr.microsoft.com/dotnet/sdk:11.0 AS build-env

WORKDIR /App

# Copy csproj
COPY *.sln .
COPY NuGet.Config Directory.Build.props global.json ./
COPY Lullaby/*.csproj Lullaby/packages.lock.json ./Lullaby/
COPY Lullaby.Database/*.csproj Lullaby.Database/packages.lock.json ./Lullaby.Database/
COPY Lullaby.Common/*.csproj Lullaby.Common/packages.lock.json ./Lullaby.Common/
COPY Lullaby.Tests/*.csproj Lullaby.Tests/packages.lock.json ./Lullaby.Tests/
COPY Lullaby.Admin/*.csproj Lullaby.Admin/packages.lock.json ./Lullaby.Admin/
COPY Lullaby.Jobs/*.csproj Lullaby.Jobs/packages.lock.json ./Lullaby.Jobs/

# Restore as distinct layers
RUN dotnet restore

# Copy everything else
COPY . ./

# Build and publish a release

# main project
WORKDIR /App/Lullaby
RUN dotnet publish -c Release -o out

# admin project
WORKDIR /App/Lullaby.Admin
RUN dotnet publish -c Release -o out

# Build runtime image
# see: https://hub.docker.com/_/microsoft-dotnet-aspnet/
FROM mcr.microsoft.com/dotnet/aspnet:11.0

WORKDIR /App/Lullaby
COPY --from=build-env /App/Lullaby/out .

WORKDIR /App/Lullaby.Admin
COPY --from=build-env /App/Lullaby.Admin/out .

WORKDIR /App/Lullaby
ENTRYPOINT ["dotnet", "Lullaby.dll"]
