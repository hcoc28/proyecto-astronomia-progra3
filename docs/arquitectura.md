# Arquitectura del Sistema

## 1. Visión general

El sistema usa **ASP.NET Core MVC** — un solo proyecto .NET que integra la capa de presentación (Razor Views), la lógica de negocio (Services), el acceso a datos (EF Core) y los endpoints API (Web API controllers). No existe un frontend separado.

```
┌──────────────────────────────────────────────────────────────────────────┐
│                             USUARIO FINAL                                │
└──────────────────────────┬───────────────────────────────────────────────┘
                           │ (navegador HTTP)
                           ▼
┌──────────────────────────────────────────────────────────────────────────┐
│                  ASP.NET CORE MVC (.NET 8)                               │
│                                                                          │
│   ┌──────────────────────────────────────────────────────────────────┐   │
│   │  Controladores MVC                                               │   │
│   │  ObjetosController  → devuelve Razor Views (páginas HTML)        │   │
│   │  GrafoController    → devuelve Razor Views + datos para JS       │   │
│   │  Api/ObjetosController → JSON (para llamadas AJAX del frontend)  │   │
│   └─────────────────────────┬────────────────────────────────────────┘   │
│                             │                                            │
│            ┌────────────────┴────────────────────┐                      │
│            ▼                                     ▼                      │
│   ┌────────────────────────┐     ┌─────────────────────────────────┐    │
│   │  Vistas (Razor/.cshtml)│     │  wwwroot/                       │    │
│   │  - Index.cshtml        │     │  - css/estilos.css              │    │
│   │  - Detalle.cshtml      │     │  - js/catalogo.js               │    │
│   │  - Busqueda.cshtml     │     │  - js/grafo-viewer.js           │    │
│   │  - Grafo.cshtml        │     │  - assets/                      │    │
│   └────────────────────────┘     └─────────────────────────────────┘    │
│                                                                          │
│   ┌──────────────────────────────────────────────────────────────────┐   │
│   │  Lógica de Negocio (Services)                                    │   │
│   │  - ObjetoService.cs      - GrafoService.cs                      │   │
│   │  - BusquedaService.cs    - ImportacionService.cs                │   │
│   └─────────┬──────────────────────────────────────────┬────────────┘   │
│             │                                          │                 │
│             ▼                                          ▼                 │
│   ┌─────────────────────┐              ┌───────────────────────────┐     │
│   │ Estructuras de Datos│              │ Capa de Datos             │     │
│   │ (EstructurasDatos/) │              │ Repositories/ + EF Core   │     │
│   │ - ListaEnlazada     │              │                           │     │
│   │ - TablaHash         │              │ Módulo de Integración     │     │
│   │ - ArbolAVL          │              │ Integracion/              │     │
│   │ - Cola              │              │ SolarSystemApiClient.cs   │     │
│   │ - Grafo             │              └─────────────┬─────────────┘     │
│   └─────────────────────┘                            │                   │
└──────────────────────────────────────────────────────┼───────────────────┘
                                                       │ SQL (EF Core)
                                                       ▼
┌──────────────────────────────────────────────────────────────────────────┐
│                      BASE DE DATOS (SQL Server)                          │
│   - objetos_astronomicos                                                 │
│   - sistemas_planetarios                                                 │
│   - relaciones (para el grafo)                                           │
│   - constelaciones                                                       │
│   - consultas_log                                                        │
└──────────────────────────────────────────────────────────────────────────┘
                                    ▲
                         (API externa / Internet)
                    Solar System OpenData API
                  https://api.le-systeme-solaire.net
```

---

## 2. Descripción de capas

### 2.1 Controladores y Vistas (presentación)

**Responsabilidad:** recibir peticiones HTTP, coordinar servicios y devolver respuestas.

ASP.NET Core MVC usa dos tipos de controladores en el mismo proyecto:

| Tipo | Hereda de | Devuelve | Uso |
|------|-----------|----------|-----|
| MVC Controller | `Controller` | `IActionResult` (View o Redirect) | Páginas completas |
| API Controller | `ControllerBase` | `IActionResult` (JSON) | Llamadas AJAX desde el JS del wwwroot |

**Vistas Razor (.cshtml):**
- `Views/Objetos/Index.cshtml` — catálogo de objetos.
- `Views/Objetos/Detalle.cshtml` — vista de objeto individual.
- `Views/Objetos/Busqueda.cshtml` — búsqueda avanzada.
- `Views/Grafo/Index.cshtml` — visualización de relaciones.
- `Views/Shared/_Layout.cshtml` — plantilla base (nav, header, footer).

**Archivos estáticos (wwwroot/):**
- `css/estilos.css` — estilos globales.
- `js/grafo-viewer.js` — dibuja el grafo con canvas/SVG.
- `js/api.js` — llamadas AJAX a los API controllers.

### 2.2 Lógica de negocio (Services)

**Responsabilidad:** coordinación de operaciones, uso de estructuras de datos, reglas de negocio.

- **Tecnología:** C# con .NET 8.
- **Subcapas internas:**
  1. **Controladores MVC:** reciben peticiones, validan y delegan a Services.
  2. **Servicios:** coordinan operaciones y usan las estructuras de datos.
  3. **Estructuras de datos:** Lista, Tabla Hash, Árbol AVL, Cola y Grafo (detalle en [estructuras.md](estructuras.md)).
  4. **Repositorios:** encapsulan consultas con **Entity Framework Core** + provider SQL Server.
  5. **Módulo de integración:** consume APIs astronómicas con `HttpClient`.

### 2.3 Base de Datos (persistencia)

**Responsabilidad:** almacenar datos de forma persistente y permitir consultas.

- **Tecnología:** SQL Server (Express o Developer edition).
- **ORM:** Entity Framework Core con `Microsoft.EntityFrameworkCore.SqlServer`.
- **Tablas principales:** ver [modelo-datos.md](modelo-datos.md).

### 2.4 Módulo de integración externa

**Responsabilidad:** obtener datos desde fuentes externas y transformarlos al modelo interno.

- **Fuentes principales:**
  - Solar System OpenData API — https://api.le-systeme-solaire.net
- **Proceso:**
  1. Petición HTTP a la API externa.
  2. Deserialización JSON → objetos C#.
  3. Validación y limpieza.
  4. Inserción en SQL Server vía EF Core.
  5. Carga en memoria dentro de las estructuras de datos.

---

## 3. Flujo de datos típico

### Ejemplo: el usuario busca un planeta por nombre

```
1. Usuario escribe "Marte" en el buscador (página Razor).
2. El formulario hace POST /Objetos/Buscar o AJAX a /api/objetos/buscar?nombre=Marte
3. ObjetosController.Buscar() recibe la petición.
4. Llama a ObjetoService.BuscarPorNombre("Marte").
5. El servicio usa TablaHash.Obtener("Marte") → O(1).
6. Si no está en hash, consulta SQL Server y actualiza el hash.
7. MVC: devuelve View("Busqueda", resultados) → Razor renderiza HTML.
   API: devuelve JSON { "nombre": "Marte", "masa": 6.39e23, ... }
8. La página muestra los datos al usuario.
```

### Ejemplo: el usuario calcula una ruta entre dos sistemas

```
1. Usuario selecciona origen = "Sol" y destino = "Proxima Centauri" en la vista.
2. AJAX: fetch('/api/grafo/ruta?origen=Sol&destino=Proxima+Centauri')
3. GrafoController (API) recibe la petición.
4. El servicio GrafoService usa BFS/Dijkstra sobre el grafo en memoria.
5. Devuelve JSON con la ruta y distancia total.
6. grafo-viewer.js dibuja la ruta en el canvas de la vista Razor.
```

---

## 4. Decisiones arquitectónicas

| Decisión | Justificación |
|----------|---------------|
| ASP.NET Core MVC | Integra frontend (Razor) y backend en un solo proyecto .NET 8. Reduce complejidad de despliegue. |
| Razor Views | HTML generado en servidor, sin necesidad de SPA ni framework JS pesado. |
| Web API controllers dentro del mismo proyecto | Permite AJAX desde el JS del wwwroot sin CORS. |
| SQL Server | Motor robusto de Microsoft, integración nativa con el ecosistema .NET y EF Core. |
| Entity Framework Core | ORM solicitado por el catedrático. Simplifica acceso a datos y migraciones. |
| Estructuras en memoria + BD | BD para persistencia; estructuras para análisis y búsquedas rápidas en caliente. |

---

## 5. Estructura del proyecto .NET

```
backend/
├── AstronomiaApp/
│   ├── Controllers/
│   │   ├── ObjetosController.cs      # MVC: devuelve Views
│   │   ├── GrafoController.cs        # MVC: devuelve Views
│   │   └── Api/
│   │       ├── ObjetosApiController.cs  # API: devuelve JSON
│   │       └── GrafoApiController.cs    # API: devuelve JSON
│   ├── Views/
│   │   ├── Objetos/
│   │   │   ├── Index.cshtml
│   │   │   ├── Detalle.cshtml
│   │   │   └── Busqueda.cshtml
│   │   ├── Grafo/
│   │   │   └── Index.cshtml
│   │   └── Shared/
│   │       └── _Layout.cshtml
│   ├── Models/                       # Entidades + ViewModels
│   ├── Services/                     # Lógica de negocio
│   ├── Repositories/                 # Acceso a datos (EF Core)
│   ├── EstructurasDatos/             # Implementaciones manuales
│   ├── Integracion/                  # SolarSystemApiClient.cs
│   ├── Data/                         # AstronomiaDbContext.cs
│   ├── wwwroot/
│   │   ├── css/
│   │   ├── js/
│   │   └── assets/
│   ├── Program.cs
│   └── appsettings.json
├── AstronomiaApp.Tests/
└── AstronomiaApp.sln
```

---

## 6. Consideraciones de despliegue

En Fase 1 y 2 todo corre en `localhost`:
- Aplicación MVC: `http://localhost:5000`
- SQL Server: `localhost,1433` (puerto estándar)

En Fase 3 se integrará completamente en la misma aplicación (sin puertos adicionales para frontend, ya que todo es un solo servidor MVC).
