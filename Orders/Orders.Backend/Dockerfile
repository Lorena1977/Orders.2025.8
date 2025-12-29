# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar toda la solución desde la raíz del repo
# Desde Orders.Backend (Root Directory), subimos dos niveles a Orders/
COPY ../../ ./

# Restaurar dependencias de la solución completa
RUN dotnet restore Orders.sln

# Publicar el backend
WORKDIR /src/Orders/Orders.Backend
RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados
COPY --from=build /app/out .

# Exponer el puerto que tu app utiliza (ajustar si es diferente)
EXPOSE 5000

# Ejecutar la aplicación
ENTRYPOINT ["dotnet", "Orders.Backend.dll"]