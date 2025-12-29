# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos la solución y los proyectos individualmente para aprovechar cache de Docker
COPY Orders.sln ./
COPY Orders.Backend/Orders.Backend.csproj Orders.Backend/
COPY Orders.Frontend/Orders.Frontend.csproj Orders.Frontend/
COPY Orders.Shared/Orders.Shared.csproj Orders.Shared/

# Restauramos los paquetes NuGet
RUN dotnet restore Orders.sln

# Copiamos todo el código fuente
COPY . .

# Publicamos el backend
WORKDIR /src/Orders.Backend
RUN dotnet publish -c Release -o /app

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

# Exponemos el puerto en el que corre tu aplicación
EXPOSE 5000

# Arrancamos la app
ENTRYPOINT ["dotnet", "Orders.Backend.dll"]