# Use official ASP.NET Core runtime as base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use SDK image to build the project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy remain source code
COPY HUBT_Social_API/ HUBT_Social_API/Identity_API
COPY HUBT_Social_Core/ HUBT_Social_Core/

# Restore dependencies
WORKDIR /src/HUBT_Social_API/Identity_API
RUN dotnet restore "Identity_API.csproj"

# Build the application
RUN dotnet build "Identity_API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Identity_API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final runtime image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Identity_API.dll"]
