# Arquitectura del Sistema

## 1. Visión general

El sistema sigue una arquitectura clásica de **3 capas** con comunicación por HTTP y un módulo adicional de integración con fuentes externas.

```
┌──────────────────────────────────────────────────────────────────────────┐
│                             USUARIO FINAL                                │
└──────────────────────────┬───────────────────────────────────────────────┘
                           │ (navegador)
                           ▼
┌──────────────────────────────────────────────────────────────────────────┐
│                            FRONTEND (WEB)                                │
│   HTML + CSS + JavaScript vanilla                                        │
│   - Vistas de catálogo, búsqueda, detalle                                │
│   - Visualización de grafo (rutas entre cuerpos celestes)                │
│   - Peticiones HTTP con fetch() al backend                               │
└──────────────────────────┬───────────────────────────────────────────────┘
                           │ HTTP / JSON
                           ▼
┌──────────────────────────────────────────────────────────────────────────┐
│                     BACKEND (C# / ASP.NET Core)                          │
│                                                                          │
│   ┌────────────────────────────────────────────────────────────────┐     │
│   │  API / Controladores (REST endpoints)                          │     │
│   └────────────────────┬────────────────────────────────────────┬──┘     │
│                        │                                        │        │
│                        ▼                                        ▼        │
│   ┌────────────────────────────────┐   ┌──────────────────────────────┐  │
│   │  Lógica de Negocio (Services)  │   │  Módulo de Integración API   │  │
│   │  - Validación                  │   │  - HttpClient                │  │
│   │  - Coordinación                │   │  - Deserialización JSON      │  │
│   └──┬────────────────────────┬────┘   └───────────────┬──────────────┘  │
│      │                        │                        │                 │
│      ▼                        ▼                        ▼                 │
│   ┌─────────────────┐   ┌───────────────────┐   ┌────────────────────┐   │
│   │ Estructuras de  │   │ Capa de Acceso a  │   │ APIs externas /    │   │
│   │ Datos           │   │ Datos (Repository)│   │ Datasets           │   │
│   │ - Lista         │   │ - EF Core         │   │ - Solar System     │   │
│   │ - Hash          │   │                   │   │   OpenData         │   │
│   │ - AVL           │   │                   │   │ - Open Astronomy   │   │
│   │ - Cola          │   │                   │   │   Catalogs         │   │
│   │ - Grafo         │   │                   │   │ - CSV / JSON       │   │
│   └─────────────────┘   └─────────┬─────────┘   └────────────────────┘   │
│                                   │                                      │
└───────────────────────────────────┼──────────────────────────────────────┘
                                    │ SQL
                                    ▼
┌──────────────────────────────────────────────────────────────────────────┐
│                        BASE DE DATOS (PostgreSQL)                        │
│   - objetos_astronomicos                                                 │
│   - sistemas_planetarios                                                 │
│   - relaciones (para el grafo)                                           │
│   - constelaciones                                                       │
│   - consultas_log                                                        │
└──────────────────────────────────────────────────────────────────────────┘
```

---

## 2. Descripción de capas

### 2.1 Frontend (capa de presentación)

**Responsabilidad:** mostrar información al usuario y capturar interacciones.

- **Tecnologías:** HTML5, CSS3, JavaScript vanilla (ES6+).
- **Componentes principales:**
  - `index.html` — página principal con catálogo.
  - `detalle.html` — vista de objeto individual.
  - `busqueda.html` — búsqueda avanzada.
  - `grafo.html` — visualización de relaciones (con canvas o SVG).
- **No contiene** lógica de negocio ni acceso directo a BD. Solo consume el backend vía `fetch()`.

### 2.2 Backend (capa de lógica y procesamiento)

**Responsabilidad:** lógica de negocio, estructuras de datos, comunicación con BD y APIs externas.

- **Tecnología:** C# con .NET 8 (ASP.NET Core Web API).
- **Subcapas internas:**
  1. **Controladores (API REST):** reciben peticiones HTTP, validan y delegan.
  2. **Servicios (lógica de negocio):** coordinan operaciones, usan las estructuras de datos.
  3. **Estructuras de datos:** Lista, Tabla Hash, Árbol AVL, Cola y Grafo (detalle en [estructuras.md](estructuras.md)).
  4. **Repositorios (acceso a datos):** encapsulan consultas usando **Entity Framework Core** con el provider de Npgsql para PostgreSQL.
  5. **Módulo de integración externa:** consume APIs astronómicas con `HttpClient`.

### 2.3 Base de Datos (capa de persistencia)

**Responsabilidad:** almacenar datos de forma persistente y permitir consultas.

- **Tecnología:** PostgreSQL 15+.
- **Tablas principales:** ver [modelo-datos.md](modelo-datos.md).
- **Relaciones:** claves foráneas entre sistemas planetarios y objetos, y tabla de relaciones para el grafo.

### 2.4 Módulo de integración externa

**Responsabilidad:** obtener datos desde fuentes externas y transformarlos al modelo interno.

- **Fuentes principales:**
  - Solar System OpenData API — https://api.le-systeme-solaire.net
  - Open Astronomy Catalogs — https://astroquery.readthedocs.io
  - Datasets CSV/JSON (NASA Exoplanet Archive como alternativa).
- **Proceso:**
  1. Petición HTTP a la API externa.
  2. Deserialización JSON → objetos C#.
  3. Validación y limpieza.
  4. Inserción en PostgreSQL.
  5. Carga en memoria dentro de las estructuras de datos.

---

## 3. Flujo de datos típico

### Ejemplo: el usuario busca un planeta por nombre

```
1. Usuario escribe "Marte" en el buscador del frontend.
2. JavaScript hace: fetch('/api/objetos/buscar?nombre=Marte')
3. El controlador BackendController recibe la petición.
4. Llama a ObjetoAstronomicoService.BuscarPorNombre("Marte").
5. El servicio usa TablaHash.Obtener("Marte") → O(1).
6. Si no está en hash, consulta PostgreSQL y actualiza el hash.
7. Devuelve JSON al frontend: { "nombre": "Marte", "masa": 6.39e23, ... }
8. El frontend pinta los datos en pantalla.
```

### Ejemplo: el usuario calcula una ruta entre dos sistemas

```
1. Usuario selecciona origen = "Sol" y destino = "Proxima Centauri".
2. fetch('/api/grafo/ruta?origen=Sol&destino=Proxima+Centauri')
3. El servicio GrafoService usa BFS/Dijkstra sobre el grafo en memoria.
4. Devuelve la ruta y la distancia total.
5. El frontend dibuja la ruta en un canvas.
```

---

## 4. Decisiones arquitectónicas

| Decisión | Justificación |
|----------|---------------|
| Arquitectura de 3 capas | Es la exigida por el PDF del curso y separa responsabilidades claramente. |
| REST sobre HTTP/JSON | Estándar de la industria, simple de consumir desde JS vanilla. |
| C# + ASP.NET Core | Requisito del curso. .NET 8 es LTS y tiene excelente soporte para PostgreSQL. |
| PostgreSQL | Requisito del curso. Soporta tipos numéricos precisos (necesarios para masas/distancias astronómicas). |
| JavaScript vanilla | Requisito del curso. Sin frameworks para mantener simplicidad y enfocarse en estructuras de datos. |
| Estructuras en memoria + BD | BD para persistencia; estructuras para análisis y búsquedas rápidas en caliente. |

---

## 5. Consideraciones de despliegue

En Fase 1 y 2 todo corre en `localhost`:
- Backend: `http://localhost:5000`
- Frontend: `http://localhost:8080`
- PostgreSQL: `localhost:5432`

En Fase 3 se verificará CORS para permitir que el frontend llame al backend desde distinto puerto.
