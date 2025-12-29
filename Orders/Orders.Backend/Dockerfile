Aquí tienes el Dockerfile completo.

IMPORTANTE: Para que este archivo funcione, DEBES ir a la configuración de Render y cambiar el Root Directory a Orders. Si dejas el Root Directory vacío o en Orders.Backend, seguirá fallando.

Dockerfile

# ---------- Etapa de Compilación (Build Stage) ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 1. Copiar el archivo de solución
# El contexto ahora es la carpeta /Orders gracias al cambio en Render
COPY Orders.sln ./

# 2. Copiar los archivos de proyecto (.csproj) para restaurar dependencias
# Se copian primero para aprovechar la caché de capas de Docker
COPY Orders.Backend/Orders.Backend.csproj Orders.Backend/
COPY Orders.Shared/Orders.Shared.csproj Orders.Shared/
COPY Orders.Frontend/Orders.Frontend.csproj Orders.Frontend/

# 3. Restaurar las dependencias de toda la solución
RUN dotnet restore Orders.sln

# 4. Copiar el resto del código fuente de todos los proyectos necesarios
COPY Orders.Backend/ Orders.Backend/
COPY Orders.Shared/ Orders.Shared/
COPY Orders.Frontend/ Orders.Frontend/

# 5. Compilar y publicar el proyecto Backend
WORKDIR /src/Orders.Backend
RUN dotnet publish Orders.Backend.csproj -c Release -o /app/publish

# ---------- Etapa de Ejecución (Runtime Stage) ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde la etapa anterior
COPY --from=build /app/publish .

# Configuración de puertos para Render
# Render asigna dinámicamente un puerto, ASP.NET lo detecta mediante esta variable
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

# Comando para iniciar la aplicación
ENTRYPOINT ["dotnet", "Orders.Backend.dll"]