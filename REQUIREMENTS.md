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

Necesario para compilar y ejecutar el backend en C#.

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

Maneja la conexión y consultas a PostgreSQL desde C# sin escribir SQL manual.

**Instalar la herramienta de migraciones (una sola vez, global):**
```bash
dotnet tool install --global dotnet-ef
```

**Verificar instalación:**
```bash
dotnet ef --version
# Esperado: Entity Framework Core .NET Command-line Tools 8.x.x
```

Los paquetes NuGet del proyecto (EF Core + provider de PostgreSQL) se instalan automáticamente al hacer `dotnet restore` dentro de la carpeta del backend — no requieren descarga manual.

---

## 4. PostgreSQL 15 o superior

Base de datos del proyecto.

**Descargar:** https://www.postgresql.org/download/windows/
- Usar el instalador de **EDB** (el más completo).
- Durante la instalación, recordar la contraseña del usuario `postgres` — la van a necesitar.
- Instalar también **pgAdmin 4** (viene incluido en el instalador de EDB).

**Verificar instalación:**
```bash
psql --version
# Esperado: psql (PostgreSQL) 15.x o 16.x
```

**Crear la base de datos del proyecto:**
```bash
psql -U postgres -c "CREATE DATABASE astronomia_db;"
```

**Cargar el esquema y datos iniciales:**
```bash
psql -U postgres -d astronomia_db -f database/schema.sql
psql -U postgres -d astronomia_db -f database/seed.sql
```

---

## 4. Visual Studio Code (recomendado)

Editor de código. Si ya tienen Visual Studio 2022 o Rider, también sirve.

**Descargar:** https://code.visualstudio.com/

**Extensiones recomendadas para instalar en VS Code:**

| Extensión | Para qué sirve |
|-----------|----------------|
| `C# Dev Kit` (Microsoft) | Soporte completo de C# |
| `SQL Tools` + `SQL Tools PostgreSQL Driver` | Consultas a PostgreSQL desde VS Code |
| `Live Server` | Servidor local para el frontend |
| `GitLens` | Historial de Git mejorado |
| `Prettier` | Formateo de HTML/CSS/JS |

Instalar desde la pestaña de Extensiones (Ctrl+Shift+X) buscando el nombre.

---

## 5. Pasos para arrancar el proyecto desde cero

### 5.1 Clonar el repositorio

```bash
git clone https://github.com/eurizar/proyecto-astronomia-progra3.git
cd proyecto-astronomia-progra3
```

### 5.2 Configurar la base de datos

```bash
psql -U postgres -c "CREATE DATABASE astronomia_db;"
psql -U postgres -d astronomia_db -f database/schema.sql
psql -U postgres -d astronomia_db -f database/seed.sql
```

### 5.3 Ejecutar el prototipo de API (prueba)

```bash
cd backend/prototipo-api
dotnet run
```

Debe mostrar los planetas del Sistema Solar en consola. Si lo hace, el entorno está bien configurado.

### 5.4 Abrir el frontend

Abrir `frontend/index.html` directamente en el navegador, o usar Live Server en VS Code (clic derecho → "Open with Live Server").

---

## 6. Herramientas opcionales (recomendadas)

| Herramienta | Uso | Descarga |
|-------------|-----|----------|
| **pgAdmin 4** | Interfaz gráfica para PostgreSQL | Viene con el instalador de PostgreSQL |
| **Postman** | Probar los endpoints del backend | https://www.postman.com/downloads/ |
| **GitHub Desktop** | Interfaz gráfica para Git | https://desktop.github.com/ |

---

## 7. Resumen de versiones mínimas

| Herramienta | Versión mínima |
|-------------|----------------|
| Git | 2.40+ |
| .NET SDK | 8.0+ |
| PostgreSQL | 15+ |
| VS Code | Cualquier versión reciente |

---

## Problemas frecuentes

**`dotnet` no se reconoce como comando:**
Cerrar y volver a abrir la terminal después de instalar .NET SDK. Si sigue sin funcionar, agregar manualmente `C:\Program Files\dotnet` a la variable de entorno `PATH`.

**`psql` no se reconoce como comando:**
Agregar `C:\Program Files\PostgreSQL\15\bin` (ajustar según versión instalada) a la variable de entorno `PATH`.

**Error de contraseña en PostgreSQL:**
Usar la contraseña que definieron al instalar. Si no la recuerdan, se puede resetear desde pgAdmin.
