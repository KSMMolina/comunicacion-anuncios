# Comunicación y Anuncios - Backend (.NET 8)

## Requisitos
- .NET SDK 8.0
- SQL Server (2019+)
- Base de datos existente con esquema esperado (no se usan migraciones)

## Configuración
1) appsettings (desarrollo)
- Crea o edita `appsettings.Development.json` con tu cadena de conexión y JWT:
{ "ConnectionStrings": { "Default": "Server=localhost;Database=comunicacion_anuncios;User Id=sa;Password=TuP4ss!;TrustServerCertificate=True" }, "Jwt": { "Issuer": "Ofirma", "Audience": "OfirmaClients", "Key": "cambia_esta_clave_en_dev", "ExpiresMinutes": 120 } }

- Variables de entorno equivalentes (opcional):
  - ConnectionStrings__Default
  - Jwt__Issuer
  - Jwt__Audience
  - Jwt__Key
  - Jwt__ExpiresMinutes

2) CORS
- Por defecto se permite `http://localhost:4200`. Ajusta en `Program.cs` si es necesario.

3) Roles mínimos
- La app espera al menos los roles `Admin` y `Resident` en `dbo.Roles`.
- SQL de ejemplo:

IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE Name = 'Admin') INSERT INTO dbo.Roles (Name) VALUES ('Admin');
IF NOT EXISTS (SELECT 1 FROM dbo.Roles WHERE Name = 'Resident') INSERT INTO dbo.Roles (Name) VALUES ('Resident');

4) Usuario administrador (rápido y sin calcular hash)
- Opción recomendada:
  1. Registra un usuario con `POST /api/public/users` (ver “Autenticación y pruebas”).
  2. Cambia su `RoleId` al del rol `Admin`:


DECLARE @UserId UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Users WHERE Email = 'admin@ofima.com'); DECLARE @AdminRole UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Roles WHERE Name = 'Admin'); UPDATE dbo.Users SET RoleId = @AdminRole WHERE Id = @UserId;


- Opción alternativa (si deseas insertar directo con hash bcrypt ya calculado):
  - La columna de contraseña es `Password`.

DECLARE @AdminRole UNIQUEIDENTIFIER = (SELECT TOP 1 Id FROM dbo.Roles WHERE Name = 'Admin'); INSERT INTO dbo.Users (FullName, Email, Password, RoleId, IsActive, CreatedAt) VALUES (N'Administrador', N'admin@ofima.com', N'<HASH_BCRYPT>', @AdminRole, 1, SYSUTCDATETIME());

  - Genera `<HASH_BCRYPT>` con la contraseña deseada (bcrypt).

Notas:
- Todos los DateTime se manejan en UTC.
- No se ejecutan migraciones (`dotnet ef ...`). Usa un esquema de BD ya existente compatible con las configuraciones EF.

## Ejecución

Opción A) Visual Studio 2022
1. Abre la solución.
2. Establece el proyecto Web API como inicio: clic derecho → __Set as Startup Project__.
3. Restaura paquetes: __Restore NuGet Packages__.
4. Inicia depuración: __Start Debugging__ (F5).
5. Abre Swagger en la URL mostrada en la salida (por ejemplo, https://localhost:7xxx/swagger).

Opción B) CLI

dotnet restore dotnet build dotnet run --project ./comunicacion-anuncios

- Observa la salida para las URLs (HTTP/HTTPS).
- Si tienes problemas con certificados en HTTPS:

dotnet dev-certs https --trust

## Autenticación y pruebas en Swagger

1) Registrar usuario (rol por defecto: Resident)
- Endpoint: `POST /api/public/users`
- Body:

{ "fullName": "Admin", "email": "admin@ofima.com", "password": "P@ssw0rd!" }

- Si necesitas que sea Admin, cambia el `RoleId` del usuario en la BD (ver sección de configuración).

2) Login
- Endpoint: `POST /api/auth/login`
- Body:

{ "email": "admin@ofirma.com", "password": "P@ssw0rd!" }

- Copia el `token` del resultado.

3) Autorizar en Swagger
- Botón “Authorize” → `Bearer {token}` → Authorize.

## Endpoints principales

- Autenticación:
  - POST `/api/auth/login`
- Usuarios públicos:
  - POST `/api/public/users` (registro)
- Anuncios (requiere JWT; Admin para escribir):
  - GET `/api/announcements` (paginado: page, size, search, fromDate, toDate, activeOnly)
  - GET `/api/announcements/{id}`
  - POST `/api/announcements` (Admin)
  - PUT `/api/announcements/{id}` (Admin)
  - PATCH `/api/announcements/{id}/toggle` (Admin)
  - POST `/api/announcements/{id}/confirm`
  - GET `/api/announcements/{id}/readers` (Admin)

## Resolución de problemas

- 500 al iniciar: verifica `ConnectionStrings:Default` y acceso a SQL Server.
- 401 en login: email/contraseña o `IsActive=1`.
- 403 en endpoints de Admin: el usuario no tiene rol `Admin`.
- Certificado HTTPS dev: `dotnet dev-certs https --trust`.
- CORS desde frontend: ajusta origen en la política `"Default"` si no usas `http://localhost:4200`.

## Notas de seguridad y despliegue
- Cambia la clave JWT por una segura y almacénala fuera del repositorio (por ejemplo, __Manage User Secrets__ o variables de entorno).
- Ajusta CORS a los orígenes reales de tu frontend.
- Revisa índices y restricciones en BD según las configuraciones EF (tablas: dbo.Users, dbo.Roles, dbo.Announcements, dbo.ReadConfirmations, dbo.Attachments).