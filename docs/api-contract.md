# Contrato de API Interna (Backend ↔ Frontend)

Este documento define los endpoints REST que expondrá el backend en C#. El frontend los consumirá vía `fetch()`. **Mientras el backend no esté listo, el frontend puede trabajar con datos mock que respeten estas estructuras.**

---

## Convenciones generales

- **Base URL (desarrollo):** `http://localhost:5000/api`
- **Formato de intercambio:** JSON en requests y responses
- **Codificación:** UTF-8
- **Nombres de campos:** camelCase en JSON (ej. `distanciaTierraAl`)
- **Fechas:** ISO 8601 (`2026-04-15T14:30:00Z`)

### Códigos de estado HTTP usados

| Código | Significado |
|--------|-------------|
| `200 OK` | Operación exitosa |
| `201 Created` | Recurso creado |
| `400 Bad Request` | Parámetros inválidos |
| `404 Not Found` | Recurso no encontrado |
| `500 Internal Server Error` | Error del servidor |

### Respuesta de error estándar

```json
{
  "error": true,
  "mensaje": "Descripcion del error",
  "codigo": "NOT_FOUND"
}
```

---

## 1. Endpoints de Objetos Astronómicos

### 1.1 Listar todos los objetos

```
GET /api/objetos
```

**Query params opcionales:**
- `tipo` — filtrar por tipo (planeta, estrella, galaxia, ...)
- `limit` — máximo de resultados (default: 100)
- `offset` — paginación

**Respuesta 200:**
```json
{
  "total": 250,
  "items": [
    {
      "id": 1,
      "nombre": "Marte",
      "tipo": "Planeta",
      "masa": 6.4171e23,
      "radio": 3389.5,
      "distanciaTierraAl": 0.0000402,
      "temperatura": 210,
      "sistema": "Sistema Solar"
    }
  ]
}
```

**Estructura usada internamente:** Lista enlazada.

---

### 1.2 Obtener objeto por ID

```
GET /api/objetos/{id}
```

**Respuesta 200:**
```json
{
  "id": 1,
  "nombre": "Marte",
  "tipo": "Planeta",
  "masa": 6.4171e23,
  "radio": 3389.5,
  "distanciaTierraAl": 0.0000402,
  "temperatura": 210,
  "luminosidad": null,
  "sistema": "Sistema Solar",
  "constelacion": null,
  "descripcion": "Planeta rojo"
}
```

**Respuesta 404:** si el ID no existe.

---

### 1.3 Buscar objeto por nombre (búsqueda rápida)

```
GET /api/objetos/buscar?nombre={texto}
```

**Comportamiento:**
- Busca coincidencias exactas primero (usa **tabla hash**).
- Si no encuentra, hace búsqueda parcial `ILIKE` en BD.

**Respuesta 200:**
```json
{
  "encontrados": 3,
  "items": [
    { "id": 1, "nombre": "Marte", "tipo": "Planeta" },
    { "id": 42, "nombre": "Marte B", "tipo": "Exoplaneta" }
  ]
}
```

**Estructura usada internamente:** Tabla Hash (manual).

---

### 1.4 Ordenar objetos

```
GET /api/objetos/ordenar?por={campo}&direccion={asc|desc}&tipo={tipo}
```

**Valores de `por`:**
- `distancia` — distancia a la Tierra
- `masa`
- `radio`
- `temperatura`
- `luminosidad`

**Respuesta 200:** mismo formato que `GET /api/objetos`, ordenado.

**Estructura usada internamente:** Árbol AVL (manual).

---

### 1.5 Búsqueda por rango

```
GET /api/objetos/rango?campo={campo}&min={valor}&max={valor}
```

**Ejemplo:** estrellas con masa entre 1e29 y 1e31 kg.

**Respuesta 200:** lista de objetos dentro del rango.

**Estructura usada internamente:** Árbol AVL (búsqueda por rango).

---

## 2. Endpoints del Grafo

### 2.1 Obtener vecinos de un objeto

```
GET /api/grafo/vecinos/{idObjeto}
```

**Respuesta 200:**
```json
{
  "origen": { "id": 1, "nombre": "Sol" },
  "vecinos": [
    { "id": 5, "nombre": "Tierra", "tipoRelacion": "orbita", "distanciaAl": 0.0000158 },
    { "id": 15, "nombre": "Proxima Centauri", "tipoRelacion": "cercania", "distanciaAl": 4.2465 }
  ]
}
```

---

### 2.2 Calcular ruta entre dos objetos

```
GET /api/grafo/ruta?origen={id|nombre}&destino={id|nombre}
```

**Respuesta 200:**
```json
{
  "origen": "Sol",
  "destino": "TRAPPIST-1",
  "distanciaTotal": 40.66,
  "saltos": 2,
  "ruta": [
    { "id": 1, "nombre": "Sol" },
    { "id": 15, "nombre": "Proxima Centauri" },
    { "id": 30, "nombre": "TRAPPIST-1" }
  ]
}
```

**Respuesta 404:** si no hay ruta posible.

**Estructura usada internamente:** Grafo (manual) + algoritmo Dijkstra.

---

### 2.3 Explorar sistema planetario completo (BFS)

```
GET /api/grafo/sistema/{idSistema}
```

**Respuesta 200:**
```json
{
  "sistema": "Sistema Solar",
  "estrellaCentral": { "id": 1, "nombre": "Sol" },
  "objetos": [
    { "id": 2, "nombre": "Mercurio" },
    { "id": 3, "nombre": "Venus" },
    { "id": 4, "nombre": "Tierra" }
  ]
}
```

**Estructura usada internamente:** Grafo (BFS).

---

## 3. Endpoints de Tipos y Catálogos

### 3.1 Listar tipos de objeto

```
GET /api/tipos
```

**Respuesta 200:**
```json
[
  { "id": 1, "nombre": "Planeta", "cantidad": 8 },
  { "id": 2, "nombre": "Estrella", "cantidad": 150 }
]
```

---

### 3.2 Listar sistemas planetarios

```
GET /api/sistemas
```

**Respuesta 200:**
```json
[
  { "id": 1, "nombre": "Sistema Solar", "estrellaCentral": "Sol", "cantidadObjetos": 8 },
  { "id": 2, "nombre": "Alpha Centauri", "estrellaCentral": "Proxima Centauri", "cantidadObjetos": 3 }
]
```

---

### 3.3 Listar constelaciones

```
GET /api/constelaciones
```

---

## 4. Endpoints de Operaciones del Sistema

### 4.1 Registrar consulta en cola

```
POST /api/consultas
```

**Body:**
```json
{
  "tipoConsulta": "analisis_sistema",
  "parametros": { "sistema": "Sistema Solar" }
}
```

**Respuesta 201:**
```json
{
  "consultaId": 42,
  "estado": "encolada",
  "posicion": 3
}
```

**Estructura usada internamente:** Cola.

---

### 4.2 Importar datos desde API externa

```
POST /api/admin/importar
```

**Body:**
```json
{
  "fuente": "solar_system_opendata",
  "tipos": ["planetas", "satelites"]
}
```

**Respuesta 200:**
```json
{
  "importados": 158,
  "actualizados": 20,
  "errores": 0,
  "duracionMs": 3421
}
```

---

## 5. Endpoint de salud (debug)

### 5.1 Health check

```
GET /api/health
```

**Respuesta 200:**
```json
{
  "status": "ok",
  "bd": "conectada",
  "objetosCargados": 250,
  "timestamp": "2026-04-15T14:30:00Z"
}
```

---

## 6. Resumen de mapeo endpoint ↔ estructura de datos

| Endpoint | Estructura interna |
|----------|--------------------|
| `GET /api/objetos` | Lista enlazada |
| `GET /api/objetos/buscar` | Tabla Hash |
| `GET /api/objetos/ordenar` | Árbol AVL |
| `GET /api/objetos/rango` | Árbol AVL |
| `GET /api/grafo/*` | Grafo |
| `POST /api/consultas` | Cola |
| (historial en UI) | Pila (frontend) |

---

## 7. CORS

Se habilitará CORS en el backend para permitir que el frontend (puerto 8080 u otro) haga peticiones al backend (puerto 5000):

```csharp
// En Program.cs
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});
```

---

## 8. Versionado

La API inicial es v1 (implícita). Si en el futuro cambia la estructura de respuesta, se versionará como `/api/v2/...`.
