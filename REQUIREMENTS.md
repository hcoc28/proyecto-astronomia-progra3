# Requisitos de instalación

Lo que cada integrante del equipo necesita instalar antes de poder trabajar en el proyecto.

---

## 1. Git

Control de versiones. Obligatorio para clonar el repositorio y colaborar.

**Descargar:** https://git-scm.com/downloads

**Verificar instalación:**
```bash
git --version
# Esperado: git version 2.x.x
```

**Configurar identidad (una sola vez):**
```bash
git config --global user.name "Tu Nombre"
git config --global user.email "tu-correo@ejemplo.com"
```

---

## 2. .NET SDK 8.0

Necesario para compilar y ejecutar el backend en C# (ASP.NET Core MVC).

**Descargar:** https://dotnet.microsoft.com/download/dotnet/8.0
- Descargar **.NET SDK** (no solo el Runtime).
- Elegir el instalador para Windows x64.

**Verificar instalación:**
```bash
dotnet --version
# Esperado: 8.0.x
```

---

## 3. Entity Framework Core (ORM)

Maneja la conexión y consultas a SQL Server desde C# sin escribir SQL manual.

**Instalar la herramienta de migraciones (una sola vez, global):**
```bash
dotnet tool install --global dotnet-ef
```

**Verificar instalación:**
```bash
dotnet ef --version
# Esperado: Entity Framework Core .NET Command-line Tools 8.x.x
```

Los paquetes NuGet del proyecto se instalan automáticamente al hacer `dotnet restore` dentro de la carpeta del backend — no requieren descarga manual.

---

## 4. SQL Server

Base de datos del proyecto. Usar la edición **Express** o **Developer** (ambas gratuitas).

### Opción A — SQL Server Express (recomendado para desarrollo)

**Descargar:** https://www.microsoft.com/es-es/sql-server/sql-server-downloads
- En la página, elegir **Express** → descargar el instalador básico.

**Verificar que el servicio esté corriendo:**
- Buscar "Servicios" en Windows.
- Confirmar que `SQL Server (SQLEXPRESS)` está en estado **En ejecución**.

### Opción B — SQL Server Developer (más completo)

**Descargar:** https://www.microsoft.com/es-es/sql-server/sql-server-downloads
- Elegir **Developer** — mismas funciones que Enterprise, solo para desarrollo.

---

## 5. SQL Server Management Studio (SSMS)

Interfaz gráfica para administrar la base de datos, ejecutar scripts y ver tablas.

**Descargar:** https://aka.ms/ssmsfullsetup

**Alternativa más ligera:** Azure Data Studio — https://aka.ms/azuredatastudio

---

## 6. Visual Studio 2022 (recomendado) o VS Code

Editor de código. Visual Studio 2022 tiene soporte nativo superior para ASP.NET Core MVC.

### Visual Studio 2022

**Descargar:** https://visualstudio.microsoft.com/es/downloads/
- Edición **Community** (gratuita para estudiantes).
- Durante la instalación, seleccionar el workload **"Desarrollo de ASP.NET y web"**.

### VS Code (alternativa)

**Descargar:** https://code.visualstudio.com/

**Extensiones recomendadas:**

| Extensión | Para qué sirve |
|-----------|----------------|
| `C# Dev Kit` (Microsoft) | Soporte completo de C# y ASP.NET Core |
| `SQL Server (mssql)` (Microsoft) | Conexión y consultas a SQL Server desde VS Code |
| `Razor+` | Resaltado de sintaxis para vistas .cshtml |
| `GitLens` | Historial de Git mejorado |
| `Prettier` | Formateo de HTML/CSS/JS |

Instalar desde la pestaña de Extensiones (Ctrl+Shift+X) buscando el nombre.

---

## 7. Pasos para arrancar el proyecto desde cero

### 7.1 Clonar el repositorio

```bash
git clone https://github.com/eurizar/proyecto-astronomia-progra3.git
cd proyecto-astronomia-progra3
```

### 7.2 Crear y configurar la base de datos

#### Paso A — Encontrar el nombre de tu servidor SQL Server

1. Abrir **SSMS** (SQL Server Management Studio).
2. En el cuadro "Nombre del servidor" del diálogo de conexión, el nombre que aparece es tu servidor (ej. `ELIZANDRO`, `DESKTOP-ABC123`, `localhost\SQLEXPRESS`).
3. Anotar ese nombre — se usará en el paso C.

> **Autenticación recomendada:** usar **Autenticación de Windows** (no requiere contraseña). Si usas autenticación SQL Server (`sa`), necesitarás la contraseña del usuario.

#### Paso B — Ejecutar los scripts SQL en orden

Con SSMS abierto y conectado a tu servidor, ejecutar los siguientes archivos **en este orden**. Importante: leer por qué existe cada uno.

---

**Script 1 — `database/schema.sql`** *(ejecutar conectado a `AstronomiaDB` o crear la BD primero)*

**¿Por qué existe?**
Define la **estructura** de la base de datos: crea las 6 tablas del sistema (`tipos_objeto`, `constelaciones`, `sistemas_planetarios`, `objetos_astronomicos`, `relaciones`, `consultas_log`), sus columnas, tipos de dato, claves primarias, claves foráneas e índices de búsqueda. Sin este script, la base de datos existe pero está vacía — no hay dónde guardar nada.

También contiene sentencias `DROP TABLE IF EXISTS` al inicio para poder volver a ejecutarlo en cualquier momento durante el desarrollo sin errores (borra y recrea las tablas limpias).

**Cómo ejecutarlo:**
1. En SSMS → **Nueva consulta**
2. Si `AstronomiaDB` no existe aún, ejecutar primero:
   ```sql
   CREATE DATABASE AstronomiaDB;
   ```
3. En el desplegable de base de datos (arriba izquierda), seleccionar **`AstronomiaDB`**
4. `Archivo → Abrir → Archivo...` → seleccionar `database/schema.sql`
5. Clic en **Ejecutar** (F5)
6. Resultado esperado: `Los comandos se han completado correctamente.`

---

**Script 2 — `database/seed.sql`** *(ejecutar después de schema.sql, en `AstronomiaDB`)*

**¿Por qué existe?**
Inserta **datos iniciales de prueba** para que la aplicación tenga información desde el primer día sin necesidad de conectarse a la API externa. Incluye: 7 tipos de objeto, 5 constelaciones, 3 sistemas planetarios, 13 objetos astronómicos (Sol, 8 planetas, Próxima Centauri, TRAPPIST-1 y 2 exoplanetas) y 8 relaciones entre ellos que forman el grafo inicial.

Sin este script, la app arrancaría con el catálogo vacío y habría que importar datos desde la API externa antes de poder probar cualquier funcionalidad.

**Cómo ejecutarlo:**
1. En SSMS → **Nueva consulta**
2. Desplegable → **`AstronomiaDB`** (verificar antes de ejecutar)
3. `Archivo → Abrir` → seleccionar `database/seed.sql`
4. Clic en **Ejecutar** (F5)
5. Resultado esperado: múltiples líneas de `(N filas afectadas)` sin errores rojos.

**Verificar que quedó bien:**
```sql
USE AstronomiaDB;
SELECT COUNT(*) AS total_objetos FROM objetos_astronomicos;
-- Debe devolver: 13
```

---

#### Paso C — Configurar la cadena de conexión

Editar `backend/AstronomiaApp/appsettings.json` — reemplazar `TU_SERVIDOR` con el nombre encontrado en el Paso A:

```json
{
  "ConnectionStrings": {
    "AstronomiaDB": "Server=TU_SERVIDOR;Database=AstronomiaDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Ejemplos según instalación:**

| Tipo de instalación | Valor de `Server` |
|--------------------|--------------------|
| SQL Server Developer (instalación por defecto) | `NOMBRE_PC` (ej. `ELIZANDRO`) |
| SQL Server Express | `NOMBRE_PC\SQLEXPRESS` |
| SQL Server local con instancia nombrada | `localhost\NOMBRE_INSTANCIA` |

> `Trusted_Connection=True` usa **Autenticación de Windows** — no requiere usuario ni contraseña en el archivo. Si tu equipo usa autenticación SQL Server, cambiar a: `User Id=sa;Password=TU_PASSWORD;`

### 7.4 Ejecutar el prototipo de API (prueba de Fase 1)

```bash
cd backend/prototipo-api
dotnet run
```

Debe mostrar los planetas del Sistema Solar en consola. Si lo hace, el entorno .NET está bien configurado.

### 7.5 Ejecutar la aplicación MVC (Fase 2 en adelante)

```bash
cd backend/AstronomiaApp
dotnet restore
dotnet run
# Abrir http://localhost:5000 en el navegador
```

---

## 8. Herramientas opcionales (recomendadas)

| Herramienta | Uso | Descarga |
|-------------|-----|----------|
| **Postman** | Probar los endpoints API del backend | https://www.postman.com/downloads/ |
| **GitHub Desktop** | Interfaz gráfica para Git | https://desktop.github.com/ |
| **Azure Data Studio** | Alternativa liviana a SSMS | https://aka.ms/azuredatastudio |

---

## 9. Resumen de versiones mínimas

| Herramienta | Versión mínima |
|-------------|----------------|
| Git | 2.40+ |
| .NET SDK | 8.0+ |
| SQL Server | 2019+ (o Express) |
| Visual Studio | 2022 Community |
| VS Code | Cualquier versión reciente |

---

## Problemas frecuentes

**`dotnet` no se reconoce como comando:**
Cerrar y volver a abrir la terminal después de instalar .NET SDK. Si sigue sin funcionar, agregar manualmente `C:\Program Files\dotnet` a la variable de entorno `PATH`.

**No se puede conectar a SQL Server:**
- Verificar que el servicio `SQL Server (SQLEXPRESS)` o `SQL Server (MSSQLSERVER)` esté corriendo en "Servicios de Windows".
- Si usas Express, cambiar la cadena de conexión a `Server=localhost\SQLEXPRESS`.
- Habilitar TCP/IP en SQL Server Configuration Manager si la conexión falla por red.

**Error de certificado SSL en la conexión:**
Agregar `TrustServerCertificate=True;` a la cadena de conexión (ya incluido en la plantilla de appsettings.json).
