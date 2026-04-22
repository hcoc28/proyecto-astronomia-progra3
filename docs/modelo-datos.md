# Modelo de Base de Datos

Base de datos: **SQL Server** (2019 o superior)
Nombre: `AstronomiaDB`
ORM: Entity Framework Core con provider `Microsoft.EntityFrameworkCore.SqlServer`

---

## 1. Diagrama Entidad-Relación (ASCII)

```
┌──────────────────────────────┐
│  tipos_objeto                │
│──────────────────────────────│
│ PK id              INT IDENTITY│
│    nombre          NVARCHAR  │  ← "Planeta", "Estrella", "Galaxia", etc.
│    descripcion     NVARCHAR(MAX)│
└──────────────┬───────────────┘
               │ 1
               │
               │ N
┌──────────────▼──────────────────────────────┐
│  objetos_astronomicos                       │
│─────────────────────────────────────────────│
│ PK id                      INT IDENTITY     │
│ FK tipo_id                 INT              │──► tipos_objeto
│    nombre                  NVARCHAR UNIQUE  │
│    masa_kg                 NUMERIC(30,5)    │
│    radio_km                NUMERIC(20,5)    │
│    distancia_tierra_al     NUMERIC(20,5)    │  (años luz)
│    temperatura_k           NUMERIC(12,2)    │
│    luminosidad             NUMERIC(20,5)    │
│ FK sistema_id              INT NULL         │──► sistemas_planetarios
│ FK constelacion_id         INT NULL         │──► constelaciones
│    descripcion             NVARCHAR(MAX)    │
│    fecha_creacion          DATETIME2        │
└──────────────┬──────────────────────────────┘
               │
               │ (origen y destino)
               │
┌──────────────▼─────────────────────────┐
│  relaciones                            │
│────────────────────────────────────────│
│ PK id                    INT IDENTITY  │
│ FK origen_id             INT           │──► objetos_astronomicos
│ FK destino_id            INT           │──► objetos_astronomicos
│    tipo_relacion         NVARCHAR(30)  │  ← "orbita", "gravitacional", "cercania"
│    distancia_al          NUMERIC(20,5) │  (años luz)
│    peso                  NUMERIC(15,5) │  (para algoritmo de grafos)
└────────────────────────────────────────┘

┌────────────────────────────────────────┐
│  sistemas_planetarios                  │
│────────────────────────────────────────│
│ PK id                    INT IDENTITY  │
│    nombre                NVARCHAR UNIQUE│
│ FK estrella_central_id   INT NULL      │──► objetos_astronomicos
│    descripcion           NVARCHAR(MAX) │
└────────────────────────────────────────┘

┌────────────────────────────────────────┐
│  constelaciones                        │
│────────────────────────────────────────│
│ PK id                    INT IDENTITY  │
│    nombre                NVARCHAR UNIQUE│
│    abreviatura           NVARCHAR(10)  │
│    descripcion           NVARCHAR(MAX) │
└────────────────────────────────────────┘

┌────────────────────────────────────────┐
│  consultas_log                         │
│────────────────────────────────────────│
│ PK id                    INT IDENTITY  │
│    usuario               NVARCHAR(50)  │
│    tipo_consulta         NVARCHAR(50)  │
│    parametros            NVARCHAR(MAX) │  ← JSON serializado
│    resultado_count       INT           │
│    fecha                 DATETIME2     │
└────────────────────────────────────────┘
```

---

## 2. Descripción de tablas

### 2.1 `tipos_objeto`
Catálogo de tipos de objetos astronómicos (planeta, estrella, galaxia, agujero negro, exoplaneta, constelación).

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | INT IDENTITY PK | Identificador único |
| `nombre` | NVARCHAR(50) | Nombre del tipo (único) |
| `descripcion` | NVARCHAR(MAX) | Descripción general |

### 2.2 `objetos_astronomicos`
Tabla principal del sistema. Cada fila es un objeto único del universo.

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | INT IDENTITY PK | Identificador único |
| `tipo_id` | INT FK | Referencia a `tipos_objeto` |
| `nombre` | NVARCHAR(100) UNIQUE | Nombre del objeto (ej. "Marte") |
| `masa_kg` | NUMERIC(30,5) | Masa en kilogramos |
| `radio_km` | NUMERIC(20,5) | Radio en kilómetros |
| `distancia_tierra_al` | NUMERIC(20,5) | Distancia a la Tierra en años luz |
| `temperatura_k` | NUMERIC(12,2) | Temperatura superficial en Kelvin |
| `luminosidad` | NUMERIC(20,5) | Luminosidad (solar = 1.0) |
| `sistema_id` | INT FK NULL | Sistema planetario al que pertenece |
| `constelacion_id` | INT FK NULL | Constelación visible |
| `descripcion` | NVARCHAR(MAX) | Texto libre |
| `fecha_creacion` | DATETIME2 | Fecha de inserción en la BD |

### 2.3 `sistemas_planetarios`
Agrupa objetos que orbitan una estrella central.

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | INT IDENTITY PK | Identificador único |
| `nombre` | NVARCHAR(100) UNIQUE | Nombre del sistema (ej. "Sistema Solar") |
| `estrella_central_id` | INT FK NULL | Estrella central del sistema |
| `descripcion` | NVARCHAR(MAX) | Texto libre |

### 2.4 `constelaciones`
Agrupaciones visuales de estrellas.

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | INT IDENTITY PK | Identificador único |
| `nombre` | NVARCHAR(100) UNIQUE | Nombre (ej. "Orion") |
| `abreviatura` | NVARCHAR(10) | Abreviatura oficial (ej. "Ori") |
| `descripcion` | NVARCHAR(MAX) | Texto libre |

### 2.5 `relaciones`
Tabla puente para el grafo — almacena aristas entre objetos.

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | INT IDENTITY PK | Identificador único |
| `origen_id` | INT FK | Objeto origen |
| `destino_id` | INT FK | Objeto destino |
| `tipo_relacion` | NVARCHAR(30) | Tipo (orbita / gravitacional / cercania) |
| `distancia_al` | NUMERIC(20,5) | Distancia en años luz |
| `peso` | NUMERIC(15,5) | Valor usado como peso en el grafo |

### 2.6 `consultas_log`
Registro de consultas del usuario (útil para estadísticas y auditoría).

| Campo | Tipo | Descripción |
|-------|------|-------------|
| `id` | INT IDENTITY PK | Identificador único |
| `usuario` | NVARCHAR(50) | Usuario o sesión |
| `tipo_consulta` | NVARCHAR(50) | Tipo (búsqueda / ordenamiento / grafo) |
| `parametros` | NVARCHAR(MAX) | JSON serializado con los parámetros |
| `resultado_count` | INT | Cantidad de resultados devueltos |
| `fecha` | DATETIME2 | Momento de la consulta |

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
- Las relaciones no pueden tener `origen_id = destino_id` (CHECK constraint).
- `nombre` es único en `objetos_astronomicos`, `sistemas_planetarios` y `constelaciones`.
- Eliminaciones en cascada se manejan en la capa de servicio (evita rutas de cascada múltiples en SQL Server).

---

## 5. Consultas representativas del sistema

```sql
-- 1. Listar todos los planetas del Sistema Solar ordenados por distancia
SELECT o.nombre, o.distancia_tierra_al
FROM objetos_astronomicos o
JOIN sistemas_planetarios s ON o.sistema_id = s.id
JOIN tipos_objeto t ON o.tipo_id = t.id
WHERE s.nombre = N'Sistema Solar' AND t.nombre = N'Planeta'
ORDER BY o.distancia_tierra_al ASC;

-- 2. Buscar objeto por nombre (exacto o parcial)
SELECT * FROM objetos_astronomicos
WHERE nombre LIKE N'%Marte%';

-- 3. Estrellas más masivas
SELECT o.nombre, o.masa_kg
FROM objetos_astronomicos o
JOIN tipos_objeto t ON o.tipo_id = t.id
WHERE t.nombre = N'Estrella'
ORDER BY o.masa_kg DESC
OFFSET 0 ROWS FETCH NEXT 10 ROWS ONLY;

-- 4. Vecinos de un objeto en el grafo
SELECT o2.nombre, r.distancia_al, r.tipo_relacion
FROM relaciones r
JOIN objetos_astronomicos o1 ON r.origen_id = o1.id
JOIN objetos_astronomicos o2 ON r.destino_id = o2.id
WHERE o1.nombre = N'Sol';
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

El volumen es moderado — SQL Server lo maneja sin problemas, y las estructuras en memoria pueden cargar el catálogo completo.

---

## 7. Cadena de conexión (appsettings.json)

```json
{
  "ConnectionStrings": {
    "AstronomiaDB": "Server=localhost;Database=AstronomiaDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

Para autenticación con usuario/contraseña SQL Server:
```json
"AstronomiaDB": "Server=localhost;Database=AstronomiaDB;User Id=sa;Password=TuPassword;TrustServerCertificate=True;"
```
