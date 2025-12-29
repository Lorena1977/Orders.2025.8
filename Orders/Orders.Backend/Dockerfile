# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

# Copiar los archivos de la solución y los proyectos
COPY Orders.Backend.sln ./
COPY Orders.Backend/Orders.Backend.csproj Orders.Backend/
COPY Orders.Shared/Orders.Shared.csproj Orders.Shared/
COPY Orders.Frontend/Orders.Frontend.csproj Orders.Frontend/

# Restaurar dependencias
RUN dotnet restore Orders.Backend.sln

# Copiar todo el código fuente
COPY . .

# Compilar y publicar la app en modo Release
RUN dotnet publish Orders.Backend/Orders.Backend.csproj -c Release -o /app/publish

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app
COPY --from=build /app/publish .

# Puerto que Render asignará dinámicamente
ENV ASPNETCORE_URLS=http://+:$PORT
EXPOSE $PORT

ENTRYPOINT ["dotnet", "Orders.Backend.dll"]