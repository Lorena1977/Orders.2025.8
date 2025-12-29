# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos de proyecto primero para restaurar dependencias
COPY Orders.Backend/Orders.Backend.csproj Orders.Backend/
COPY Orders.Frontend/Orders.Frontend.csproj Orders.Frontend/
COPY Orders.Shared/Orders.Shared.csproj Orders.Shared/

# Restaurar paquetes
RUN dotnet restore Orders.Backend/Orders.Backend.csproj

# Copiar todo el código fuente
COPY . .

# Construir y publicar el backend
WORKDIR /src/Orders.Backend
RUN dotnet publish Orders.Backend.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/publish .

# Puerto que expondrá Render
EXPOSE 5000

# Ejecutable
ENTRYPOINT ["dotnet", "Orders.Backend.dll"]