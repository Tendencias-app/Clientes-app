# Dockerfile para construir y ejecutar la aplicación Blazor (.NET 10)
# Uso recomendado (ajusta PROJECT y PROJECT_NAME si es necesario):
# docker build --build-arg PROJECT=./Path/To/YourProject.csproj --build-arg PROJECT_NAME=YourAssembly.dll -t carsattm-client .
# docker run -p 8080:80 carsattm-client

# Stage de build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

ARG PROJECT=CarsATTM-CustomerApp.csproj
ARG PROJECT_NAME=CarsATTM-CustomerApp.dll

WORKDIR /src

# Copiar todo el código de la solución
COPY . .

# Restaurar y publicar el proyecto indicado
RUN dotnet restore "$PROJECT"
RUN dotnet publish "$PROJECT" -c Release -o /app/publish /p:UseAppHost=false

# Stage de runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

ENTRYPOINT ["dotnet", "CarsATTM-CustomerApp.dll"]
