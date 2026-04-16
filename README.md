# Proyecto Astronomía — Programación III

Sistema de Exploración y Análisis de Objetos Astronómicos.

**Universidad Mariano Gálvez de Guatemala — Campus Cobán**
**Ingeniería en Sistemas y Ciencias de la Computación — Programación III**

---

## 1. Descripción del proyecto

Aplicación web que permite explorar, analizar y visualizar información sobre objetos astronómicos (planetas, estrellas, galaxias, exoplanetas, constelaciones y agujeros negros). El sistema consume información desde APIs públicas de datos astronómicos, la procesa mediante estructuras de datos avanzadas y la almacena en PostgreSQL para consulta y análisis.

Este proyecto corresponde a la **Variante 8** del documento de requerimientos del curso.

### Objetivos

- **General:** integrar los conocimientos del curso (estructuras de datos, POO, persistencia, arquitectura por capas) en un sistema funcional que organice y analice información astronómica real.
- **Específicos:**
  - Implementar al menos 5 estructuras de datos del curso (con mínimo 2 manuales).
  - Consumir información desde una API pública astronómica.
  - Persistir los datos en una base de datos PostgreSQL.
  - Ofrecer una interfaz web para búsqueda, ordenamiento y visualización.
  - Aplicar control de versiones mediante GitHub y ramas por funcionalidad.

---

## 2. Integrantes del equipo

| Nombre | Rol principal | Rol secundario | Usuario GitHub |
|--------|---------------|----------------|----------------|
| *(Integrante 1)* | Backend & Estructuras de Datos | Integración API externa | *(pendiente)* |
| *(Integrante 2)* | Base de Datos & API Externa | Backend (Hash, Grafo) | *(pendiente)* |
| *(Integrante 3)* | Frontend & Documentación | QA / Pruebas | `eurizar` |

> Completar esta tabla con los nombres reales y usuarios de GitHub del equipo.

---

## 3. Estructura del repositorio

```
/
├── backend/              # Código C# (.NET) — lógica, estructuras de datos, API REST
│   └── prototipo-api/    # Prueba inicial de consumo de API externa (Fase 1)
├── frontend/             # Aplicación web — HTML, CSS, JavaScript
├── database/             # Scripts SQL — schema, seed, migraciones
├── docs/                 # Documentación del proyecto
│   ├── arquitectura.md
│   ├── estructuras.md
│   ├── modelo-datos.md
│   ├── api-contract.md
│   ├── convenciones.md
│   └── revision-1.md
├── PlanDeTrabajo.md      # Plan general del proyecto
├── RequerimientoFinal.pdf  # Documento original del curso
├── .gitignore
└── README.md
```

---

## 4. Tecnologías utilizadas

| Capa | Tecnología |
|------|------------|
| Backend | C# con .NET 8 (ASP.NET Core Web API) |
| Base de datos | PostgreSQL 15+ |
| Frontend | HTML5 + CSS3 + JavaScript (vanilla) |
| API externa | Solar System OpenData API (https://api.le-systeme-solaire.net) |
| Control de versiones | Git + GitHub |

---

## 5. Instrucciones de instalación

### Requisitos previos

- [.NET SDK 8.0](https://dotnet.microsoft.com/download) o superior
- [PostgreSQL 15+](https://www.postgresql.org/download/)
- Navegador web moderno (Chrome, Edge, Firefox)
- Git

### Pasos

**1. Clonar el repositorio**
```bash
git clone https://github.com/eurizar/proyecto-astronomia-progra3.git
cd proyecto-astronomia-progra3
```

**2. Configurar la base de datos**
```bash
# Crear base de datos en PostgreSQL
psql -U postgres -c "CREATE DATABASE astronomia_db;"

# Ejecutar scripts
psql -U postgres -d astronomia_db -f database/schema.sql
psql -U postgres -d astronomia_db -f database/seed.sql
```

**3. Ejecutar el backend**
```bash
cd backend
dotnet restore
dotnet run
# El backend queda escuchando en http://localhost:5000
```

**4. Ejecutar el frontend**
Abrir `frontend/index.html` en el navegador, o servirlo con un servidor local:
```bash
cd frontend
python -m http.server 8080
# Abrir http://localhost:8080
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
