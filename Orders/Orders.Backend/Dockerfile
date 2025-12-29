# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar la solución completa y los proyectos
COPY ../Orders.sln .
COPY ../Orders.Backend/Orders.Backend.csproj Orders.Backend/
COPY ../Orders.Frontend/Orders.Frontend.csproj Orders.Frontend/
COPY ../Orders.Shared/ Orders.Shared/

# Restaurar dependencias de la solución
RUN dotnet restore Orders.sln

# Build y publish del backend
WORKDIR /src/Orders.Backend
RUN dotnet publish Orders.Backend.csproj -c Release -o /app/publish

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde la etapa de build
COPY --from=build /app/publish .

# Exponer el puerto que usará Render
EXPOSE 5000

# Comando de entrada para iniciar la app
ENTRYPOINT ["dotnet", "Orders.Backend.dll"]