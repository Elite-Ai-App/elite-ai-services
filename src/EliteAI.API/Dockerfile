FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_ENVIRONMENT=Staging

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src


COPY ["src/EliteAI.API/EliteAI.API.csproj", "src/EliteAI.API/"]
COPY ["src/EliteAI.Infrastructure/EliteAI.Infrastructure.csproj", "src/EliteAI.Infrastructure/"]
COPY ["src/EliteAI.Domain/EliteAI.Domain.csproj", "src/EliteAI.Domain/"]
COPY ["src/EliteAI.Application/EliteAI.Application.csproj", "src/EliteAI.Application/"]

# Restore dependencies
RUN dotnet restore "src/EliteAI.API/EliteAI.API.csproj"

# Copy the rest of the code
COPY . .

# Build and publish
RUN dotnet publish "src/EliteAI.API/EliteAI.API.csproj" -c Release -o /app/out -v diag


# ----------- RUNTIME STAGE -----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/out .

CMD ["dotnet", "EliteAI.API.dll"] 