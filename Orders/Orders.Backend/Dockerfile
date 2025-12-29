# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar proyectos necesarios dentro del contexto
COPY Orders.Backend.csproj ./
COPY Orders.Shared/ Orders.Shared/

# Restaurar dependencias
RUN dotnet restore Orders.Backend.csproj

# Copiar todo el código de backend y shared
COPY . .

# Publicar backend
RUN dotnet publish -c Release -o /app/out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar archivos publicados
COPY --from=build /app/out .

EXPOSE 5000
ENTRYPOINT ["dotnet", "Orders.Backend.dll"]