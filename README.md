# Proyecto Astronomía — Programación III

Sistema de Exploración y Análisis de Objetos Astronómicos.

**Universidad Mariano Gálvez de Guatemala — Campus Cobán**
**Ingeniería en Sistemas y Ciencias de la Computación — Programación III**

---

## 1. Descripción del proyecto

Aplicación web que permite explorar, analizar y visualizar información sobre objetos astronómicos (planetas, estrellas, galaxias, exoplanetas, constelaciones y agujeros negros). El sistema consume información desde APIs públicas de datos astronómicos, la procesa mediante estructuras de datos avanzadas y la almacena en SQL Server para consulta y análisis.

Este proyecto corresponde a la **Variante 8** del documento de requerimientos del curso.

### Objetivos

- **General:** integrar los conocimientos del curso (estructuras de datos, POO, persistencia, arquitectura por capas) en un sistema funcional que organice y analice información astronómica real.
- **Específicos:**
  - Implementar al menos 5 estructuras de datos del curso (con mínimo 2 manuales).
  - Consumir información desde una API pública astronómica.
  - Persistir los datos en una base de datos SQL Server.
  - Ofrecer una interfaz web (Razor Views) para búsqueda, ordenamiento y visualización.
  - Aplicar control de versiones mediante GitHub y ramas por funcionalidad.

---

## 2. Integrantes del equipo

| Nombre | Rol principal | Rol secundario | Usuario GitHub |
|--------|---------------|----------------|----------------|
| *(Integrante 1)* | Backend & Estructuras de Datos | Integración API externa | *(pendiente)* |
| *(Integrante 2)* | Base de Datos & API Externa | Backend (Hash, Grafo) | *(pendiente)* |
| *(Integrante 3)* | Vistas & Documentación | QA / Pruebas | *(pendiente)* |

> Completar esta tabla con los nombres reales y usuarios de GitHub del equipo.

---

## 3. Estructura del repositorio

```
/
├── backend/              # Proyecto ASP.NET Core MVC (.NET 8)
│   ├── AstronomiaApp/    # Aplicación principal (Controllers, Views, Services, etc.)
│   └── prototipo-api/    # Prueba inicial de consumo de API externa (Fase 1)
├── database/             # Scripts SQL — schema, seed
├── docs/                 # Documentación del proyecto
│   ├── arquitectura.md
│   ├── estructuras.md
│   ├── modelo-datos.md
│   ├── api-contract.md
│   ├── convenciones.md
│   └── revision-1.md
├── PlanDeTrabajo.md      # Plan general del proyecto
├── REQUIREMENTS.md       # Guía de instalación
├── .gitignore
└── README.md
```

> **Nota:** no existe carpeta `/frontend` separada. Las vistas Razor (`.cshtml`), CSS y JavaScript están integrados dentro del proyecto MVC en `backend/AstronomiaApp/Views/` y `backend/AstronomiaApp/wwwroot/`.

---

## 4. Tecnologías utilizadas

| Capa | Tecnología |
|------|------------|
| Framework web | ASP.NET Core MVC (.NET 8) |
| Vistas (UI) | Razor Views (.cshtml) + HTML5 + CSS3 + JavaScript |
| Base de datos | SQL Server 2019+ |
| ORM | Entity Framework Core (`Microsoft.EntityFrameworkCore.SqlServer`) |
| API externa | Solar System OpenData API (https://api.le-systeme-solaire.net) |
| Control de versiones | Git + GitHub |

---

## 5. Instrucciones de instalación

### Requisitos previos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download) o superior
- [SQL Server 2019+](https://www.microsoft.com/es-es/sql-server/sql-server-downloads) (Express o Developer — gratuitos)
- [SQL Server Management Studio (SSMS)](https://aka.ms/ssmsfullsetup) o Azure Data Studio
- Navegador web moderno (Chrome, Edge, Firefox)
- Git

### Pasos

**1. Clonar el repositorio**
```bash
git clone https://github.com/eurizar/proyecto-astronomia-progra3.git
cd proyecto-astronomia-progra3
```

**2. Crear la base de datos**

En SSMS o Azure Data Studio:
```sql
CREATE DATABASE AstronomiaDB;
```

Luego ejecutar los scripts:
```
database/schema.sql   → crea las tablas
database/seed.sql     → carga datos iniciales
```

**3. Configurar la cadena de conexión**

Editar `backend/AstronomiaApp/appsettings.json`:
```json
{
  "ConnectionStrings": {
    "AstronomiaDB": "Server=localhost;Database=AstronomiaDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**4. Ejecutar el backend**
```bash
cd backend/AstronomiaApp
dotnet restore
dotnet run
# La aplicación queda en http://localhost:5000
```

**5. Abrir en el navegador**
```
http://localhost:5000
```

> Nota: las instrucciones detalladas se completarán en cada fase del proyecto.

---

## 6. Estado actual del proyecto

- [x] **Fase 1 — Planeación y Diseño** (en curso)
- [ ] Fase 2 — Implementación del Backend
- [ ] Fase 3 — Integración completa
- [ ] Fase 4 — Pruebas, documentación y presentación final

Ver [PlanDeTrabajo.md](PlanDeTrabajo.md) para el cronograma completo.

---

## 7. Flujo de trabajo con Git

- `main` — rama de producción, protegida. Solo merges por Pull Request.
- `develop` — rama de integración. Todo avance se fusiona aquí primero.
- `feature/<nombre>` — una rama por funcionalidad. Se crea desde `develop`.

Ver [docs/convenciones.md](docs/convenciones.md) para detalles completos.

---

## 8. Licencia

Proyecto académico — uso exclusivo para fines educativos del curso de Programación III.
