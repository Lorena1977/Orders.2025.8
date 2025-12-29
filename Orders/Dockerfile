# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar toda la solución (Orders.Backend, Orders.Frontend, Orders.Shared)
COPY . .

# Restaurar dependencias de toda la solución
RUN dotnet restore Orders.sln

# Publicar el backend
WORKDIR /src/Orders/Orders.Backend
RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados
COPY --from=build /app/out .

# Exponer el puerto de la app
EXPOSE 5000

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "Orders.Backend.dll"]