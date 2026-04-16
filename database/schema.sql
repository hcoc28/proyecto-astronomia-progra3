-- =====================================================================
-- Script de creación de esquema - Proyecto Astronomía Programación III
-- Base de datos: PostgreSQL 15+
-- =====================================================================

-- Limpieza (solo para desarrollo)
DROP TABLE IF EXISTS consultas_log CASCADE;
DROP TABLE IF EXISTS relaciones CASCADE;
DROP TABLE IF EXISTS objetos_astronomicos CASCADE;
DROP TABLE IF EXISTS sistemas_planetarios CASCADE;
DROP TABLE IF EXISTS constelaciones CASCADE;
DROP TABLE IF EXISTS tipos_objeto CASCADE;

-- =====================================================================
-- TABLA: tipos_objeto
-- Catálogo de tipos (planeta, estrella, galaxia, etc.)
-- =====================================================================
CREATE TABLE tipos_objeto (
    id           SERIAL PRIMARY KEY,
    nombre       VARCHAR(50)  NOT NULL UNIQUE,
    descripcion  TEXT
);

-- =====================================================================
-- TABLA: sistemas_planetarios
-- Agrupa objetos que orbitan una estrella central
-- =====================================================================
CREATE TABLE sistemas_planetarios (
    id                     SERIAL PRIMARY KEY,
    nombre                 VARCHAR(100) NOT NULL UNIQUE,
    estrella_central_id    INT,
    descripcion            TEXT
);

-- =====================================================================
-- TABLA: constelaciones
-- Agrupaciones visuales de estrellas (catálogo IAU)
-- =====================================================================
CREATE TABLE constelaciones (
    id            SERIAL PRIMARY KEY,
    nombre        VARCHAR(100) NOT NULL UNIQUE,
    abreviatura   VARCHAR(10),
    descripcion   TEXT
);

-- =====================================================================
-- TABLA: objetos_astronomicos
-- Tabla principal del sistema
-- =====================================================================
CREATE TABLE objetos_astronomicos (
    id                     SERIAL PRIMARY KEY,
    tipo_id                INT NOT NULL,
    nombre                 VARCHAR(100) NOT NULL UNIQUE,
    masa_kg                NUMERIC(30, 5),
    radio_km               NUMERIC(20, 5),
    distancia_tierra_al    NUMERIC(20, 5),
    temperatura_k          NUMERIC(12, 2),
    luminosidad            NUMERIC(20, 5),
    sistema_id             INT,
    constelacion_id        INT,
    descripcion            TEXT,
    fecha_creacion         TIMESTAMP DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT fk_objetos_tipo
        FOREIGN KEY (tipo_id) REFERENCES tipos_objeto(id),

    CONSTRAINT fk_objetos_sistema
        FOREIGN KEY (sistema_id) REFERENCES sistemas_planetarios(id)
        ON DELETE SET NULL,

    CONSTRAINT fk_objetos_constelacion
        FOREIGN KEY (constelacion_id) REFERENCES constelaciones(id)
        ON DELETE SET NULL
);

-- Agregar FK de estrella_central_id una vez creada la tabla objetos_astronomicos
ALTER TABLE sistemas_planetarios
    ADD CONSTRAINT fk_sistemas_estrella
    FOREIGN KEY (estrella_central_id) REFERENCES objetos_astronomicos(id)
    ON DELETE SET NULL;

-- =====================================================================
-- TABLA: relaciones (aristas del grafo)
-- =====================================================================
CREATE TABLE relaciones (
    id              SERIAL PRIMARY KEY,
    origen_id       INT NOT NULL,
    destino_id      INT NOT NULL,
    tipo_relacion   VARCHAR(30) NOT NULL,
    distancia_al    NUMERIC(20, 5),
    peso            NUMERIC(15, 5),

    CONSTRAINT fk_relaciones_origen
        FOREIGN KEY (origen_id) REFERENCES objetos_astronomicos(id)
        ON DELETE CASCADE,

    CONSTRAINT fk_relaciones_destino
        FOREIGN KEY (destino_id) REFERENCES objetos_astronomicos(id)
        ON DELETE CASCADE,

    CONSTRAINT chk_relaciones_no_self
        CHECK (origen_id <> destino_id)
);

-- =====================================================================
-- TABLA: consultas_log (auditoría de consultas)
-- =====================================================================
CREATE TABLE consultas_log (
    id                SERIAL PRIMARY KEY,
    usuario           VARCHAR(50),
    tipo_consulta     VARCHAR(50) NOT NULL,
    parametros        JSONB,
    resultado_count   INT,
    fecha             TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- =====================================================================
-- ÍNDICES para optimizar consultas frecuentes
-- =====================================================================
CREATE INDEX idx_objetos_nombre      ON objetos_astronomicos(nombre);
CREATE INDEX idx_objetos_tipo        ON objetos_astronomicos(tipo_id);
CREATE INDEX idx_objetos_distancia   ON objetos_astronomicos(distancia_tierra_al);
CREATE INDEX idx_objetos_sistema     ON objetos_astronomicos(sistema_id);
CREATE INDEX idx_relaciones_origen   ON relaciones(origen_id);
CREATE INDEX idx_relaciones_destino  ON relaciones(destino_id);

-- =====================================================================
-- FIN del script de esquema
-- =====================================================================
