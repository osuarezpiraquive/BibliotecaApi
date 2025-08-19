# BibliotecaApi 📚  
**Prueba Técnica Backend Junior – TuBoleta**

Este proyecto incluye una API REST para la gestión de una biblioteca y un servicio en segundo plano para el procesamiento de archivos de log y análisis de métricas.

---

## Características principales

### 1. API REST

- CRUD completo de libros y categorías.
- Autenticación por JWT.
- Búsqueda de libros con filtros.
- Documentación de API con Swagger.


### 2. Background Service para procesamiento de logs 

- Servicio en segundo plano que monitorea una carpeta (`Logs/`).
- Lee archivos `.log`, los analiza y extrae métricas.
- Almacena las métricas en una tabla de base de datos.
- Mueve los archivos ya procesados a `Logs/Processed/`.


---

## Tecnologías utilizadas

- [.NET 8](https://dotnet.microsoft.com/)
- ASP.NET Core Web API
- Entity Framework Core
- MySQL
- JWT Authentication
- Swagger / OpenAPI
- BackgroundService

---

## 📦 Estructura del proyecto

BibliotecaApi/
- Controllers/ # Controladores REST
- Models/ # Entidades: Libro, Categoria, LogMetric
- Data/ # DbContext y configuraciones
- Services/ # Servicio de procesamiento de logs
- Logs/ # Carpeta monitoreada por el BackgroundService
    - Processed/ # Archivos ya procesados
- script.sql # Script SQL para crear base de datos y tabla LogMetrics
- Program.cs # Configuración del host y servicios
- appsettings.json # Configuración general y cadena de conexión
