# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore Api/Api.csproj
RUN dotnet publish Api/Api.csproj -c Release -o /app/publish

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Railway usually routes to 8080; if $PORT exists, use it.
ENV ASPNETCORE_URLS=http://0.0.0.0:8080

EXPOSE 8080

# If PORT is set by the platform, override at runtime
ENTRYPOINT ["sh", "-c", "dotnet Api.dll --urls http://0.0.0.0:${PORT:-8080}"]