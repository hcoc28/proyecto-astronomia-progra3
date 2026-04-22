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

En SSMS o Azure Data Studio, conectarse a `localhost` y ejecutar:

```sql
CREATE DATABASE AstronomiaDB;
```

Luego, abrir y ejecutar en orden:
1. `database/schema.sql` — crea las tablas
2. `database/seed.sql` — carga datos iniciales

### 7.3 Configurar la cadena de conexión

Editar `backend/AstronomiaApp/appsettings.json` con los datos de tu instancia SQL Server:

```json
{
  "ConnectionStrings": {
    "AstronomiaDB": "Server=localhost;Database=AstronomiaDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

> Si usas SQL Server Express la instancia puede llamarse `localhost\SQLEXPRESS`. Ajustar `Server=localhost\SQLEXPRESS`.

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
