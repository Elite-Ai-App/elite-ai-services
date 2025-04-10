# Build stage
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore dependencies
COPY ["src/EliteAI.API/EliteAI.API.csproj", "src/EliteAI.API/"]
COPY ["src/EliteAI.Application/EliteAI.Application.csproj", "src/EliteAI.Application/"]
COPY ["src/EliteAI.Domain/EliteAI.Domain.csproj", "src/EliteAI.Domain/"]
COPY ["src/EliteAI.Infrastructure/EliteAI.Infrastructure.csproj", "src/EliteAI.Infrastructure/"]

# Create a solution file
RUN dotnet new sln -n EliteAI
RUN dotnet sln add src/EliteAI.API/EliteAI.API.csproj
RUN dotnet sln add src/EliteAI.Application/EliteAI.Application.csproj
RUN dotnet sln add src/EliteAI.Domain/EliteAI.Domain.csproj
RUN dotnet sln add src/EliteAI.Infrastructure/EliteAI.Infrastructure.csproj

RUN dotnet restore "EliteAI.sln"

# Copy the rest of the code
COPY . .

# Build the application
WORKDIR "/src/src/EliteAI.API"
RUN dotnet build "EliteAI.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
WORKDIR "/src/src/EliteAI.API"
RUN dotnet publish "EliteAI.API.csproj" -c Release -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment variables
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:80

# Expose port 80
EXPOSE 80

# Start the application
ENTRYPOINT ["dotnet", "EliteAI.API.dll"] 