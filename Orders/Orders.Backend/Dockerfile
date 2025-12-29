# Imagen base para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los proyectos usando rutas relativas
COPY ../Orders.Backend/Orders.Backend.csproj Orders.Backend/
COPY ../Orders.Shared/ Orders.Shared/
COPY ../Orders.Frontend/ Orders.Frontend/

# Copiar la solución completa y restaurar dependencias
COPY ../Orders.sln .
RUN dotnet restore Orders.sln

# Build y publish del backend
WORKDIR /src/Orders.Backend
RUN dotnet publish Orders.Backend.csproj -c Release -o /app/publish

# Imagen runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 5000
ENTRYPOINT ["dotnet", "Orders.Backend.dll"]