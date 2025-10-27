# Comunicación y Anuncios - Backend (.NET 8)

> **Metadatos del reto**: módulo "Comunicación y Anuncios" con autenticación JWT obligatoria, ProblemDetails para errores (401 cuando credenciales erradas), paginación/filtros, roles Admin/Resident, confirmación de lectura, auditoría (CreatedAt/By, UpdatedAt/By) y seeds obligatorios.

## Requisitos
- [.NET SDK 8.0](https://dotnet.microsoft.com/)
- SQL Server 2019 o superior accesible desde la máquina local
- (Opcional) [dotnet-ef](https://learn.microsoft.com/ef/core/cli/dotnet) para gestionar migraciones

## Estructura Clean Architecture
```
backend/
  Communication.Announcements.sln
  src/
    Domain/                -> Entidades y contratos de dominio
    Application/           -> CQRS (MediatR), DTOs, validaciones, mapeos
    Infrastructure/        -> EF Core, JWT, repositorios y seeds
    WebApi/                -> Controllers, Program.cs, middleware ProblemDetails
  tests/
    Application.Tests/     -> Pruebas unitarias de Application
```

## Configuración
1. Ajusta la cadena de conexión en `src/WebApi/appsettings.json` (`ConnectionStrings:Default`).
2. (Opcional) crea variables de entorno o `Secret Manager` para la clave JWT en producción.

## Migraciones y base de datos
Ejecuta los siguientes comandos desde `/backend`:
```bash
dotnet tool restore
cd src/WebApi
dotnet ef migrations add InitialCreate
cd ../..
dotnet ef database update --project src/WebApi/WebApi.csproj --startup-project src/WebApi/WebApi.csproj
```
La aplicación ejecuta automáticamente `DbInitializer` al iniciar para sembrar roles, usuarios y anuncios de ejemplo.

### Usuarios semillas
| Rol    | Email              | Password   |
|--------|--------------------|------------|
| Admin  | admin@ofirma.com   | P@ssw0rd!  |
| Resident | juan@ofirma.com | P@ssw0rd!  |
| Resident | maria@ofirma.com | P@ssw0rd! |

## Ejecución
Desde `/backend`:
```bash
dotnet restore
cd src/WebApi
dotnet run
```
La API se expone en `http://localhost:5100` y `https://localhost:7100`. Swagger está disponible en `/swagger` (incluye soporte JWT Bearer).

## Autenticación y pruebas
1. Solicita token con `POST /api/v1/auth/login` usando alguno de los usuarios semillas.
2. Copia el `token` devuelto y úsalo en Swagger (botón **Authorize** -> `Bearer {token}`).
3. Prueba los endpoints de anuncios:
   - `GET /api/v1/announcements`
   - `GET /api/v1/announcements/{id}`
   - `POST /api/v1/announcements` (Admin)
   - `PUT /api/v1/announcements/{id}` (Admin)
   - `PATCH /api/v1/announcements/{id}/toggle` (Admin)
   - `POST /api/v1/announcements/{id}/confirm`
   - `GET /api/v1/announcements/{id}/readers` (Admin)

## Tests
```bash
cd backend/tests/Application.Tests
dotnet test
```

## Notas de diseño
- Implementación CQRS con MediatR y `ValidationBehavior` para FluentValidation.
- Filtros y paginación vía `GetAnnouncementsQuery` (solo Residentes ven anuncios activos).
- `GlobalExceptionHandler` centraliza respuestas RFC7807, incluyendo validaciones, 401 y 404.
- Auditoría básica (`CreatedAt/By`, `UpdatedAt/By`) en entidades auditable.
- Seeds cubren roles, usuarios, anuncios y confirmaciones de lectura para validación rápida.
