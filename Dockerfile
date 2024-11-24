# Learn about building .NET container images:
# https://github.com/dotnet/dotnet-docker/blob/main/samples/README.md
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /App

# Copy project file and restore as distinct layers
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out LightManager.csproj


# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build /App/out .
USER $APP_UID
ENTRYPOINT ["dotnet", "LightManager.dll"]