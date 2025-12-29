# ---------- Build Stage ----------
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# No incluyas "Orders/" en la ruta, porque ya estás dentro de ella
COPY Orders.sln ./

# Copiar proyectos (rutas relativas a la raíz del contexto)
COPY Orders.Shared/Orders.Shared.csproj Orders.Shared/
COPY Orders.Backend/Orders.Backend.csproj Orders.Backend/
COPY Orders.Frontend/Orders.Frontend.csproj Orders.Frontend/

RUN dotnet restore Orders.sln

# Copiar el código fuente
COPY Orders.Shared/ Orders.Shared/
COPY Orders.Backend/ Orders.Backend/
COPY Orders.Frontend/ Orders.Frontend/

# Publicar backend
WORKDIR /src/Orders.Backend
RUN dotnet publish Orders.Backend.csproj -c Release -o /app/publish

# ---------- Runtime Stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:10000
EXPOSE 10000

ENTRYPOINT ["dotnet", "Orders.Backend.dll"]