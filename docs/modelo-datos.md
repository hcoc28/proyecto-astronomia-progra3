# Modelo de Base de Datos

Base de datos: **PostgreSQL 15+**
Nombre: `astronomia_db`

---

## 1. Diagrama Entidad-Relación (ASCII)

```
┌──────────────────────────────┐
│  tipos_objeto                │
│──────────────────────────────│
│ PK id              SERIAL    │
│    nombre          VARCHAR   │  ← "Planeta", "Estrella", "Galaxia", etc.
│    descripcion     TEXT      │
└──────────────┬───────────────┘
               │ 1
               │
               │ N
┌──────────────▼──────────────────────────────┐
│  objetos_astronomicos                       │
│─────────────────────────────────────────────│
│ PK id                      SERIAL           │
│ FK tipo_id                 INT              │──► tipos_objeto
│    nombre                  VARCHAR UNIQUE   │
│    masa_kg                 NUMERIC(30,5)    │
│    radio_km                NUMERIC(20,5)    │
│    distancia_tierra_al     NUMERIC(20,5)    │  (años luz)
│    temperatura_k           NUMERIC(12,2)    │
│    luminosidad             NUMERIC(20,5)    │
│ FK sistema_id              INT NULL         │──► sistemas_planetarios
│ FK constelacion_id         INT NULL         │──► constelaciones
│    descripcion             TEXT             │
│    fecha_creacion          TIMESTAMP        │
└──────────────┬──────────────────────────────┘
               │
               │ (origen y destino)
               │
┌──────────────▼─────────────────────────┐
│  relaciones                            │
│────────────────────────────────────────│
│ PK id                    SERIAL        │
│ FK origen_id             INT           │──► objetos_astronomicos
│ FK destino_id            INT           │──► objetos_astronomicos
│    tipo_relacion         VARCHAR       │  ← "orbita", "gravitacional", "cercania"
│    distancia_al          NUMERIC(20,5) │  (años luz)
│    peso                  NUMERIC(15,5) │  (para algoritmo de grafos)
└────────────────────────────────────────┘

┌────────────────────────────────────────┐
│  sistemas_planetarios                  │
│────────────────────────────────────────│
│ PK id                    SERIAL        │
│    nombre                VARCHAR UNIQUE│
│ FK estrella_central_id   INT NULL      │──► objetos_astronomicos
│    descripcion           TEXT          │
└────────────────────────────────────────┘

┌────────────────────────────────────────┐
│  constelaciones                        │
│────────────────────────────────────────│
│ PK id                    SERIAL        │
│    nombre                VARCHAR UNIQUE│
│    abreviatura           VARCHAR(10)   │
│    descripcion           TEXT          │
└────────────────────────────────────────┘

┌────────────────────────────────────────┐
│  consultas_log                         │
│────────────────────────────────────────│
│ PK id                    SERIAL        │
│    usuario               VARCHAR       │
│    tipo_consulta         VARCHAR       │
│    parametros            JSONB         │
│    resultado_count       INT           │
│    fecha                 TIMESTAMP     │
└────────────────────────────────────────┘
```

---

## 2. Descripción de tablas

### 2.1 `tipos_objeto`
Catálogo de tipos de objetos astronómicos (planeta, estrella, galaxia, agujero negro, exoplaneta, constelación).

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | SERIAL PK | Identificador único |
| `nombre` | VARCHAR(50) | Nombre del tipo (único) |
| `descripcion` | TEXT | Descripción general |

### 2.2 `objetos_astronomicos`
Tabla principal del sistema. Cada fila es un objeto único del universo.

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | SERIAL PK | Identificador único |
| `tipo_id` | INT FK | Referencia a `tipos_objeto` |
| `nombre` | VARCHAR(100) UNIQUE | Nombre del objeto (ej. "Marte") |
| `masa_kg` | NUMERIC(30,5) | Masa en kilogramos (notación científica soportada) |
| `radio_km` | NUMERIC(20,5) | Radio en kilómetros |
| `distancia_tierra_al` | NUMERIC(20,5) | Distancia a la Tierra en años luz |
| `temperatura_k` | NUMERIC(12,2) | Temperatura superficial en Kelvin |
| `luminosidad` | NUMERIC(20,5) | Luminosidad (solar = 1.0) |
| `sistema_id` | INT FK NULL | Sistema planetario al que pertenece |
| `constelacion_id` | INT FK NULL | Constelación visible |
| `descripcion` | TEXT | Texto libre |
| `fecha_creacion` | TIMESTAMP | Fecha de inserción en la BD |

### 2.3 `sistemas_planetarios`
Agrupa objetos que orbitan una estrella central.

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | SERIAL PK | Identificador único |
| `nombre` | VARCHAR(100) UNIQUE | Nombre del sistema (ej. "Sistema Solar") |
| `estrella_central_id` | INT FK NULL | Estrella central del sistema |
| `descripcion` | TEXT | Texto libre |

### 2.4 `constelaciones`
Agrupaciones visuales de estrellas.

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | SERIAL PK | Identificador único |
| `nombre` | VARCHAR(100) UNIQUE | Nombre (ej. "Orion") |
| `abreviatura` | VARCHAR(10) | Abreviatura oficial (ej. "Ori") |
| `descripcion` | TEXT | Texto libre |

### 2.5 `relaciones`
Tabla puente para el grafo — almacena aristas entre objetos.

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | SERIAL PK | Identificador único |
| `origen_id` | INT FK | Objeto origen |
| `destino_id` | INT FK | Objeto destino |
| `tipo_relacion` | VARCHAR(30) | Tipo (orbita / gravitacional / cercania) |
| `distancia_al` | NUMERIC(20,5) | Distancia en años luz |
| `peso` | NUMERIC(15,5) | Valor usado como peso en el grafo |

### 2.6 `consultas_log`
Registro de consultas del usuario (útil para estadísticas y auditoría).

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | SERIAL PK | Identificador único |
| `usuario` | VARCHAR(50) | Usuario o sesión |
| `tipo_consulta` | VARCHAR(50) | Tipo (búsqueda / ordenamiento / grafo) |
| `parametros` | JSONB | Parámetros enviados |
| `resultado_count` | INT | Cantidad de resultados devueltos |
| `fecha` | TIMESTAMP | Momento de la consulta |

---

## 3. Índices

Para acelerar consultas frecuentes:

- `idx_objetos_nombre` en `objetos_astronomicos(nombre)` — búsqueda por nombre.
- `idx_objetos_tipo` en `objetos_astronomicos(tipo_id)` — filtrado por tipo.
- `idx_objetos_distancia` en `objetos_astronomicos(distancia_tierra_al)` — ordenamiento.
- `idx_relaciones_origen` en `relaciones(origen_id)` — consultas de grafo.
- `idx_relaciones_destino` en `relaciones(destino_id)` — consultas de grafo.

---

## 4. Reglas de integridad

- Todo objeto astronómico debe tener un `tipo_id` válido.
- Las relaciones no pueden tener `origen_id = destino_id` (se valida con CHECK).
- `nombre` es único en `objetos_astronomicos`, `sistemas_planetarios` y `constelaciones`.
- `ON DELETE CASCADE` en relaciones: si se elimina un objeto, sus aristas también.
- `ON DELETE SET NULL` para `sistema_id` y `constelacion_id` en objetos (no se pierde el objeto si se borra su sistema).

---

## 5. Consultas representativas del sistema

```sql
-- 1. Listar todos los planetas del Sistema Solar ordenados por distancia
SELECT o.nombre, o.distancia_tierra_al
FROM objetos_astronomicos o
JOIN sistemas_planetarios s ON o.sistema_id = s.id
JOIN tipos_objeto t ON o.tipo_id = t.id
WHERE s.nombre = 'Sistema Solar' AND t.nombre = 'Planeta'
ORDER BY o.distancia_tierra_al ASC;

-- 2. Buscar objeto por nombre (exacto o parcial)
SELECT * FROM objetos_astronomicos
WHERE nombre ILIKE '%Marte%';

-- 3. Estrellas más masivas
SELECT nombre, masa_kg
FROM objetos_astronomicos o
JOIN tipos_objeto t ON o.tipo_id = t.id
WHERE t.nombre = 'Estrella'
ORDER BY masa_kg DESC
LIMIT 10;

-- 4. Vecinos de un objeto en el grafo
SELECT o2.nombre, r.distancia_al, r.tipo_relacion
FROM relaciones r
JOIN objetos_astronomicos o1 ON r.origen_id = o1.id
JOIN objetos_astronomicos o2 ON r.destino_id = o2.id
WHERE o1.nombre = 'Sol';
```

---

## 6. Volumen estimado

| Tabla | Registros estimados |
|-------|---------------------|
| `tipos_objeto` | ~10 |
| `objetos_astronomicos` | 1,000 – 5,000 |
| `sistemas_planetarios` | 50 – 200 |
| `constelaciones` | 88 (catálogo IAU oficial) |
| `relaciones` | 2,000 – 10,000 |
| `consultas_log` | crece con el uso |

El volumen es moderado — PostgreSQL lo maneja sin problemas, y las estructuras en memoria pueden cargar el catálogo completo.
