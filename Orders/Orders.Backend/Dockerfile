# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos de proyecto al contenedor
COPY Orders.sln .
COPY Orders.Backend/Orders.Backend.csproj Orders.Backend/
COPY Orders.Frontend/Orders.Frontend.csproj Orders.Frontend/
COPY Orders.Shared/ Orders.Shared/

# Restaurar dependencias
RUN dotnet restore Orders.sln

# Copiar todo el código fuente
COPY . .

# Build y publicación
WORKDIR /src/Orders.Backend
RUN dotnet publish Orders.Backend.csproj -c Release -o /app --no-restore

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app .

# Puerto que expondrá Render
EXPOSE 5000

# Punto de entrada
ENTRYPOINT ["dotnet", "Orders.Backend.dll"]