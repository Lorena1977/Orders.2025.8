# ---------- Etapa de Compilación ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiamos la solución (Docker ya está dentro de la carpeta Orders)
COPY Orders.sln ./

# 2. Copiamos los archivos de proyecto usando rutas relativas
# CORRECTO: Orders.Shared/ (porque estamos dentro de Orders)
# INCORRECTO: Orders/Orders.Shared/ (esto buscaría una carpeta Orders dentro de Orders)
COPY Orders.Shared/Orders.Shared.csproj Orders.Shared/
COPY Orders.Backend/Orders.Backend.csproj Orders.Backend/
COPY Orders.Frontend/Orders.Frontend.csproj Orders.Frontend/

RUN dotnet restore Orders.sln

# 3. Copiamos el resto del código
COPY Orders.Shared/ Orders.Shared/
COPY Orders.Backend/ Orders.Backend/
COPY Orders.Frontend/ Orders.Frontend/

# 4. Publicar
WORKDIR /src/Orders.Backend
RUN dotnet publish Orders.Backend.csproj -c Release -o /app/publish

# ---------- Etapa de Ejecución ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Render usa el puerto 10000 por defecto
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Orders.Backend.dll"]