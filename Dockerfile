# ---------- Build ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# copy everything
COPY . .

# restore & publish only the API project
RUN dotnet restore Api/Api.csproj
RUN dotnet publish Api/Api.csproj -c Release -o /app/publish

# ---------- Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

# Railway exposes $PORT
ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

EXPOSE 8080
ENTRYPOINT ["dotnet", "Api.dll"]