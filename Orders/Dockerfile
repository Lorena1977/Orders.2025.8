# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo los proyectos necesarios
COPY Orders/Orders.Backend/Orders.Backend.csproj Orders/Orders.Backend/
COPY Orders/Orders.Shared/Orders.Shared.csproj Orders/Orders.Shared/

# Restaurar solo backend (y sus dependencias)
WORKDIR /src/Orders/Orders.Backend
RUN dotnet restore Orders.Backend.csproj

# Copiar todo el código del backend y shared
COPY Orders/Orders.Backend/ Orders/Orders.Backend/
COPY Orders/Orders.Shared/ Orders/Orders.Shared/

# Publicar backend
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