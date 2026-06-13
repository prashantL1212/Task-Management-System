# syntax=docker/dockerfile:1

# ---------- build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files first to leverage Docker layer caching on restore.
COPY ["src/TMS.API/TMS.API.csproj", "src/TMS.API/"]
COPY ["src/TMS.Application/TMS.Application.csproj", "src/TMS.Application/"]
COPY ["src/TMS.Infrastructure/TMS.Infrastructure.csproj", "src/TMS.Infrastructure/"]
COPY ["src/TMS.Domain/TMS.Domain.csproj", "src/TMS.Domain/"]
COPY ["src/TMS.Shared/TMS.Shared.csproj", "src/TMS.Shared/"]
RUN dotnet restore "src/TMS.API/TMS.API.csproj"

# Copy the rest of the source and publish.
COPY . .
RUN dotnet publish "src/TMS.API/TMS.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

# ---------- runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080
ENTRYPOINT ["dotnet", "TMS.API.dll"]
