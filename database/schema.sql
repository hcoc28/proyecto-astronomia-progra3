-- =====================================================================
-- Script de creación de esquema - Proyecto Astronomía Programación III
-- Base de datos: SQL Server (2019+)
-- Nombre BD: AstronomiaDB
-- =====================================================================

-- Limpieza (solo para desarrollo — ejecutar en orden inverso por FK)
IF OBJECT_ID('dbo.consultas_log',         'U') IS NOT NULL DROP TABLE dbo.consultas_log;
IF OBJECT_ID('dbo.relaciones',            'U') IS NOT NULL DROP TABLE dbo.relaciones;
IF OBJECT_ID('dbo.objetos_astronomicos',  'U') IS NOT NULL DROP TABLE dbo.objetos_astronomicos;
IF OBJECT_ID('dbo.sistemas_planetarios',  'U') IS NOT NULL DROP TABLE dbo.sistemas_planetarios;
IF OBJECT_ID('dbo.constelaciones',        'U') IS NOT NULL DROP TABLE dbo.constelaciones;
IF OBJECT_ID('dbo.tipos_objeto',          'U') IS NOT NULL DROP TABLE dbo.tipos_objeto;
GO

-- =====================================================================
-- TABLA: tipos_objeto
-- Catálogo de tipos (planeta, estrella, galaxia, etc.)
-- =====================================================================
CREATE TABLE tipos_objeto (
    id           INT IDENTITY(1,1) PRIMARY KEY,
    nombre       NVARCHAR(50)   NOT NULL UNIQUE,
    descripcion  NVARCHAR(MAX)
);
GO

-- =====================================================================
-- TABLA: sistemas_planetarios
-- Agrupa objetos que orbitan una estrella central
-- =====================================================================
CREATE TABLE sistemas_planetarios (
    id                     INT IDENTITY(1,1) PRIMARY KEY,
    nombre                 NVARCHAR(100) NOT NULL UNIQUE,
    estrella_central_id    INT,           -- FK hacia objetos_astronomicos (se agrega luego)
    descripcion            NVARCHAR(MAX)
);
GO

-- =====================================================================
-- TABLA: constelaciones
-- Agrupaciones visuales de estrellas (catálogo IAU)
-- =====================================================================
CREATE TABLE constelaciones (
    id            INT IDENTITY(1,1) PRIMARY KEY,
    nombre        NVARCHAR(100) NOT NULL UNIQUE,
    abreviatura   NVARCHAR(10),
    descripcion   NVARCHAR(MAX)
);
GO

-- =====================================================================
-- TABLA: objetos_astronomicos
-- Tabla principal del sistema
-- =====================================================================
CREATE TABLE objetos_astronomicos (
    id                     INT IDENTITY(1,1) PRIMARY KEY,
    tipo_id                INT           NOT NULL,
    nombre                 NVARCHAR(100) NOT NULL UNIQUE,
    masa_kg                NUMERIC(30, 5),
    radio_km               NUMERIC(20, 5),
    distancia_tierra_al    NUMERIC(20, 5),
    temperatura_k          NUMERIC(12, 2),
    luminosidad            NUMERIC(20, 5),
    sistema_id             INT,
    constelacion_id        INT,
    descripcion            NVARCHAR(MAX),
    fecha_creacion         DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT fk_objetos_tipo
        FOREIGN KEY (tipo_id) REFERENCES tipos_objeto(id),

    CONSTRAINT fk_objetos_sistema
        FOREIGN KEY (sistema_id) REFERENCES sistemas_planetarios(id),

    CONSTRAINT fk_objetos_constelacion
        FOREIGN KEY (constelacion_id) REFERENCES constelaciones(id)
);
GO

-- Agregar FK de estrella_central_id una vez creada la tabla objetos_astronomicos
ALTER TABLE sistemas_planetarios
    ADD CONSTRAINT fk_sistemas_estrella
    FOREIGN KEY (estrella_central_id) REFERENCES objetos_astronomicos(id);
GO

-- =====================================================================
-- TABLA: relaciones (aristas del grafo)
-- =====================================================================
CREATE TABLE relaciones (
    id              INT IDENTITY(1,1) PRIMARY KEY,
    origen_id       INT           NOT NULL,
    destino_id      INT           NOT NULL,
    tipo_relacion   NVARCHAR(30)  NOT NULL,
    distancia_al    NUMERIC(20, 5),
    peso            NUMERIC(15, 5),

    CONSTRAINT fk_relaciones_origen
        FOREIGN KEY (origen_id) REFERENCES objetos_astronomicos(id),

    CONSTRAINT fk_relaciones_destino
        FOREIGN KEY (destino_id) REFERENCES objetos_astronomicos(id),

    CONSTRAINT chk_relaciones_no_self
        CHECK (origen_id <> destino_id)
);
GO

-- =====================================================================
-- TABLA: consultas_log (auditoría de consultas)
-- =====================================================================
CREATE TABLE consultas_log (
    id                INT IDENTITY(1,1) PRIMARY KEY,
    usuario           NVARCHAR(50),
    tipo_consulta     NVARCHAR(50)  NOT NULL,
    parametros        NVARCHAR(MAX),        -- JSON serializado como texto
    resultado_count   INT,
    fecha             DATETIME2 DEFAULT GETDATE()
);
GO

-- =====================================================================
-- ÍNDICES para optimizar consultas frecuentes
-- =====================================================================
CREATE INDEX idx_objetos_nombre      ON objetos_astronomicos(nombre);
CREATE INDEX idx_objetos_tipo        ON objetos_astronomicos(tipo_id);
CREATE INDEX idx_objetos_distancia   ON objetos_astronomicos(distancia_tierra_al);
CREATE INDEX idx_objetos_sistema     ON objetos_astronomicos(sistema_id);
CREATE INDEX idx_relaciones_origen   ON relaciones(origen_id);
CREATE INDEX idx_relaciones_destino  ON relaciones(destino_id);
GO

-- =====================================================================
-- FIN del script de esquema
-- =====================================================================
