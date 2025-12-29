# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Ahora estamos parados en la carpeta "Orders" (gracias al Root Directory de Render)
# Copiamos la solución
COPY Orders.sln ./

# Copiamos los archivos de proyecto manteniendo la estructura de carpetas
COPY Orders.Backend/Orders.Backend.csproj Orders.Backend/
COPY Orders.Shared/Orders.Shared.csproj Orders.Shared/
COPY Orders.Frontend/Orders.Frontend.csproj Orders.Frontend/

# Restaurar dependencias de toda la solución
RUN dotnet restore Orders.sln

# Copiar el resto del código fuente
COPY Orders.Backend/ Orders.Backend/
COPY Orders.Shared/ Orders.Shared/
COPY Orders.Frontend/ Orders.Frontend/

# Build y publish del backend
WORKDIR /src/Orders.Backend
RUN dotnet publish Orders.Backend.csproj -c Release -o /app/publish

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados
COPY --from=build /app/publish .

# Render suele usar la variable de entorno PORT, ASP.NET la reconoce así:
ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Orders.Backend.dll"]