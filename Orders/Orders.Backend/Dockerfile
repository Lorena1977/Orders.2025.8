# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiar la solución (Docker ya está en /Orders)
COPY Orders.sln ./

# 2. Copiar proyectos (.csproj)
# NOTA: Sin el prefijo "Orders/"
COPY Orders.Shared/Orders.Shared.csproj Orders.Shared/
COPY Orders.Backend/Orders.Backend.csproj Orders.Backend/
COPY Orders.Frontend/Orders.Frontend.csproj Orders.Frontend/

# 3. Restaurar dependencias
RUN dotnet restore Orders.sln

# 4. Copiar el resto del código fuente
COPY Orders.Shared/ Orders.Shared/
COPY Orders.Backend/ Orders.Backend/
COPY Orders.Frontend/ Orders.Frontend/

# 5. Publicar el Backend
WORKDIR /src/Orders.Backend
RUN dotnet publish Orders.Backend.csproj -c Release -o /app/publish

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Configuración de puerto para Render
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Orders.Backend.dll"]