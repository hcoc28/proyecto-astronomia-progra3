-- =====================================================================
-- schema.sql - Proyecto Astronomia Programacion III
-- Base de datos: AstronomiaDB (SQL Server 2019+)
-- =====================================================================

USE AstronomiaDB;
GO

-- Limpieza: quitar FK circular primero, luego tablas en orden
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'fk_sistemas_estrella')
    ALTER TABLE dbo.sistemas_planetarios DROP CONSTRAINT fk_sistemas_estrella;
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'fk_relaciones_origen')
    ALTER TABLE dbo.relaciones DROP CONSTRAINT fk_relaciones_origen;
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'fk_relaciones_destino')
    ALTER TABLE dbo.relaciones DROP CONSTRAINT fk_relaciones_destino;
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'fk_objetos_tipo')
    ALTER TABLE dbo.objetos_astronomicos DROP CONSTRAINT fk_objetos_tipo;
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'fk_objetos_sistema')
    ALTER TABLE dbo.objetos_astronomicos DROP CONSTRAINT fk_objetos_sistema;
IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'fk_objetos_constelacion')
    ALTER TABLE dbo.objetos_astronomicos DROP CONSTRAINT fk_objetos_constelacion;
GO

DROP TABLE IF EXISTS dbo.consultas_log;
DROP TABLE IF EXISTS dbo.relaciones;
DROP TABLE IF EXISTS dbo.objetos_astronomicos;
DROP TABLE IF EXISTS dbo.sistemas_planetarios;
DROP TABLE IF EXISTS dbo.constelaciones;
DROP TABLE IF EXISTS dbo.tipos_objeto;
GO

-- =====================================================================
-- TABLA: tipos_objeto
-- =====================================================================
CREATE TABLE tipos_objeto (
    id          INT IDENTITY(1,1) PRIMARY KEY,
    nombre      NVARCHAR(50)  NOT NULL UNIQUE,
    descripcion NVARCHAR(MAX)
);
GO

-- =====================================================================
-- TABLA: sistemas_planetarios
-- =====================================================================
CREATE TABLE sistemas_planetarios (
    id                  INT IDENTITY(1,1) PRIMARY KEY,
    nombre              NVARCHAR(100) NOT NULL UNIQUE,
    estrella_central_id INT,
    descripcion         NVARCHAR(MAX)
);
GO

-- =====================================================================
-- TABLA: constelaciones
-- =====================================================================
CREATE TABLE constelaciones (
    id          INT IDENTITY(1,1) PRIMARY KEY,
    nombre      NVARCHAR(100) NOT NULL UNIQUE,
    abreviatura NVARCHAR(10),
    descripcion NVARCHAR(MAX)
);
GO

-- =====================================================================
-- TABLA: objetos_astronomicos
-- =====================================================================
CREATE TABLE objetos_astronomicos (
    id                  INT IDENTITY(1,1) PRIMARY KEY,
    tipo_id             INT           NOT NULL,
    nombre              NVARCHAR(100) NOT NULL UNIQUE,
    masa_kg             FLOAT,
    radio_km            FLOAT,
    distancia_tierra_al FLOAT,
    temperatura_k       FLOAT,
    luminosidad         FLOAT,
    sistema_id          INT,
    constelacion_id     INT,
    descripcion         NVARCHAR(MAX),
    fecha_creacion      DATETIME2 DEFAULT GETDATE(),

    CONSTRAINT fk_objetos_tipo
        FOREIGN KEY (tipo_id) REFERENCES tipos_objeto(id),
    CONSTRAINT fk_objetos_sistema
        FOREIGN KEY (sistema_id) REFERENCES sistemas_planetarios(id),
    CONSTRAINT fk_objetos_constelacion
        FOREIGN KEY (constelacion_id) REFERENCES constelaciones(id)
);
GO

ALTER TABLE sistemas_planetarios
    ADD CONSTRAINT fk_sistemas_estrella
    FOREIGN KEY (estrella_central_id) REFERENCES objetos_astronomicos(id);
GO

-- =====================================================================
-- TABLA: relaciones (aristas del grafo)
-- =====================================================================
CREATE TABLE relaciones (
    id            INT IDENTITY(1,1) PRIMARY KEY,
    origen_id     INT          NOT NULL,
    destino_id    INT          NOT NULL,
    tipo_relacion NVARCHAR(30) NOT NULL,
    distancia_al  FLOAT,
    peso          FLOAT,

    CONSTRAINT fk_relaciones_origen
        FOREIGN KEY (origen_id) REFERENCES objetos_astronomicos(id),
    CONSTRAINT fk_relaciones_destino
        FOREIGN KEY (destino_id) REFERENCES objetos_astronomicos(id),
    CONSTRAINT chk_relaciones_no_self
        CHECK (origen_id <> destino_id)
);
GO

-- =====================================================================
-- TABLA: consultas_log
-- =====================================================================
CREATE TABLE consultas_log (
    id              INT IDENTITY(1,1) PRIMARY KEY,
    usuario         NVARCHAR(50),
    tipo_consulta   NVARCHAR(50) NOT NULL,
    parametros      NVARCHAR(MAX),
    resultado_count INT,
    fecha           DATETIME2 DEFAULT GETDATE()
);
GO

-- Indices
CREATE INDEX idx_objetos_nombre    ON objetos_astronomicos(nombre);
CREATE INDEX idx_objetos_tipo      ON objetos_astronomicos(tipo_id);
CREATE INDEX idx_objetos_distancia ON objetos_astronomicos(distancia_tierra_al);
CREATE INDEX idx_objetos_sistema   ON objetos_astronomicos(sistema_id);
CREATE INDEX idx_relaciones_origen ON relaciones(origen_id);
CREATE INDEX idx_relaciones_destino ON relaciones(destino_id);
GO
