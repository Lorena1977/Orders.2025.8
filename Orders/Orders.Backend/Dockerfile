# Stage 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar toda la solución desde la raíz del repo
# El contexto en Render es Orders/Orders.Backend, por eso usamos ../../ para subir a Orders/
COPY ../../ ./

# Restaurar dependencias
WORKDIR /src/Orders/Orders.Backend
RUN dotnet restore

# Publicar en modo Release
RUN dotnet publish -c Release -o /app/out

# Stage 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Puerto que usará la app (ajusta si tu app usa otro)
EXPOSE 5000

ENTRYPOINT ["dotnet", "Orders.Backend.dll"]