# Comunicación y Anuncios - Backend (.NET 8)

## Requisitos
- .NET SDK 8.0
- SQL Server (2019+)
- Base de datos existente: `comunicacion_anuncios` (NO migraciones, NO `EnsureCreated`)

## Configuración
1. Edita `appsettings.json`:
   - `"ConnectionStrings:Default"` apunta a tu instancia y BD `comunicacion_anuncios`.
   - Sección `"Jwt"` con `Issuer`, `Audience`, `Key` (cambia la clave en desarrollo), `ExpiresMinutes`.
2. Asegúrate de que los DateTime del servidor se interpreten en UTC (el código usa `DateTime.UtcNow`).

Ejemplo `appsettings.json` (ajusta según tu entorno):
{ "ConnectionStrings": { "Default": "Server=localhost;Database=comunicacion_anuncios;User Id=sa;Password=<YourStrong!Passw0rd>;TrustServerCertificate=True" }, "Jwt": { "Issuer": "Ofirma", "Audience": "OfirmaClients", "Key": "<dev_key_change_me>", "ExpiresMinutes": 120 } }

## Usuario Admin manual
Inserta un usuario en `dbo.Users` con `RoleId` del rol `Admin` y un hash bcrypt válido.

Ejemplo (hash de bcrypt para contraseña `P@ssw0rd!`):

DECLARE @AdminRole UNIQUEIDENTIFIER = (SELECT Id FROM dbo.Roles WHERE Name = 'Admin');
INSERT INTO dbo.Users (FullName, Email, PasswordHash, RoleId, IsActive) VALUES (N'Administrador', N'admin@ofirma.com', N'$2a$10$8gkYgL7u2RkKQG1dZrZJrO9eKfT2xH4YcQmG1mW6mJc5lM6yV7lmy', @AdminRole, 1);

## Ejecución
1. Restaurar paquetes:

## dotnet restore
2. Ejecutar el proyecto Presentation (WebApi):

## dotnet run
3. Abrir Swagger: `https://localhost:PORT/swagger` (o `http://localhost:PORT/swagger`).
4. Autenticarse:
- `POST /api/auth/login` con body:
  ```json
  { "email": "admin@ofirma.com", "password": "P@ssw0rd!" }
  ```
- Copiar `token` resultante.
5. Autorizar en Swagger: botón Authorize → `Bearer {token}`.
6. Probar endpoints:
- `GET /api/announcements` (paginado: `page`, `size`, filtros `search`, `fromDate`, `toDate`, `activeOnly`)
- `POST /api/announcements` (Admin)
- `PUT /api/announcements/{id}`
- `PATCH /api/announcements/{id}/toggle`
- `POST /api/announcements/{id}/confirm`
- `GET /api/announcements/{id}/readers` (Admin)

## Comportamiento funcional
- Paginación ordenada por `CreatedAt` DESC.
- Rol `Resident`: sólo ve anuncios activos.
- Confirmación de lectura crea registro único (id anuncio + id usuario).
- Todos los `DateTime` manejados en UTC.
- Política `AdminOnly` basada en claim `role = "Admin"`.

## Notas
- No ejecutar comandos de migración (`dotnet ef migrations add`, `dotnet ef database update`) — la BD ya existe.
- Validaciones y errores se devuelven como ProblemDetails (RFC7807).
- Cambia la clave JWT por una segura antes de producción.
- Asegura índices y constraints iguales a la definición SQL original.

## Troubleshooting rápido
- 401 en login: verificar email, hash bcrypt y que `IsActive = 1`.
- 404 en anuncios: comprobar que el `Id` existe y que el usuario tiene permisos.
- Token expirado: renovar con `POST /api/auth/login`.

