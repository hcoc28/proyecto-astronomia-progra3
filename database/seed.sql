-- =====================================================================
-- Script de datos iniciales - Proyecto Astronomía Programación III
-- Base de datos: SQL Server
-- Ejecutar DESPUÉS de schema.sql
-- =====================================================================

-- ---------------------------------------------------------------------
-- Tipos de objeto
-- ---------------------------------------------------------------------
INSERT INTO tipos_objeto (nombre, descripcion) VALUES
    (N'Planeta',        N'Cuerpo celeste que orbita una estrella'),
    (N'Estrella',       N'Esfera de plasma unida por gravedad propia'),
    (N'Galaxia',        N'Sistema de estrellas, gas y polvo interestelar'),
    (N'Agujero Negro',  N'Region del espacio-tiempo con gravedad extrema'),
    (N'Exoplaneta',     N'Planeta fuera del Sistema Solar'),
    (N'Satelite',       N'Cuerpo que orbita un planeta'),
    (N'Asteroide',      N'Cuerpo rocoso menor del sistema solar');
GO

-- ---------------------------------------------------------------------
-- Constelaciones (muestra)
-- ---------------------------------------------------------------------
INSERT INTO constelaciones (nombre, abreviatura, descripcion) VALUES
    (N'Orion',          N'Ori', N'Constelacion prominente del cielo de invierno'),
    (N'Osa Mayor',      N'UMa', N'Contiene el Carro o Cacerola'),
    (N'Canis Major',    N'CMa', N'Contiene a Sirio, la estrella mas brillante'),
    (N'Centauro',       N'Cen', N'Contiene al sistema Alpha Centauri'),
    (N'Casiopea',       N'Cas', N'Forma de W caracteristica');
GO

-- ---------------------------------------------------------------------
-- Sistemas planetarios (sin estrella central aun, se actualiza despues)
-- ---------------------------------------------------------------------
INSERT INTO sistemas_planetarios (nombre, descripcion) VALUES
    (N'Sistema Solar',         N'Sistema donde se encuentra la Tierra'),
    (N'Alpha Centauri',        N'Sistema triple, el mas cercano al Sol'),
    (N'Sistema TRAPPIST-1',    N'Sistema con 7 exoplanetas terrestres');
GO

-- ---------------------------------------------------------------------
-- Objetos astronomicos — estrellas centrales primero
-- ---------------------------------------------------------------------
INSERT INTO objetos_astronomicos
    (tipo_id, nombre, masa_kg, radio_km, distancia_tierra_al, temperatura_k, luminosidad, sistema_id, descripcion)
VALUES
    ((SELECT id FROM tipos_objeto WHERE nombre=N'Estrella'),
     N'Sol', 1.989e30, 696340, 0.0000158, 5778, 1.0,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema Solar'),
     N'Estrella del Sistema Solar'),

    ((SELECT id FROM tipos_objeto WHERE nombre=N'Estrella'),
     N'Proxima Centauri', 2.446e29, 107000, 4.2465, 3042, 0.0017,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Alpha Centauri'),
     N'Estrella mas cercana al Sol'),

    ((SELECT id FROM tipos_objeto WHERE nombre=N'Estrella'),
     N'TRAPPIST-1', 1.79e29, 84000, 40.66, 2566, 0.000553,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema TRAPPIST-1'),
     N'Enana roja ultrafria con 7 exoplanetas');
GO

-- Actualizar sistemas con estrella central
UPDATE sistemas_planetarios SET estrella_central_id = (SELECT id FROM objetos_astronomicos WHERE nombre=N'Sol')              WHERE nombre=N'Sistema Solar';
UPDATE sistemas_planetarios SET estrella_central_id = (SELECT id FROM objetos_astronomicos WHERE nombre=N'Proxima Centauri') WHERE nombre=N'Alpha Centauri';
UPDATE sistemas_planetarios SET estrella_central_id = (SELECT id FROM objetos_astronomicos WHERE nombre=N'TRAPPIST-1')       WHERE nombre=N'Sistema TRAPPIST-1';
GO

-- Planetas del Sistema Solar
INSERT INTO objetos_astronomicos
    (tipo_id, nombre, masa_kg, radio_km, distancia_tierra_al, temperatura_k, sistema_id, descripcion)
VALUES
    ((SELECT id FROM tipos_objeto WHERE nombre=N'Planeta'),
     N'Mercurio', 3.3011e23, 2439.7, 0.00000915, 440,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema Solar'),
     N'Planeta mas cercano al Sol'),

    ((SELECT id FROM tipos_objeto WHERE nombre=N'Planeta'),
     N'Venus', 4.8675e24, 6051.8, 0.00000423, 737,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema Solar'),
     N'Planeta mas caliente del Sistema Solar'),

    ((SELECT id FROM tipos_objeto WHERE nombre=N'Planeta'),
     N'Tierra', 5.972e24, 6371.0, 0.0, 288,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema Solar'),
     N'Unico planeta conocido con vida'),

    ((SELECT id FROM tipos_objeto WHERE nombre=N'Planeta'),
     N'Marte', 6.4171e23, 3389.5, 0.0000402, 210,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema Solar'),
     N'Planeta rojo'),

    ((SELECT id FROM tipos_objeto WHERE nombre=N'Planeta'),
     N'Jupiter', 1.898e27, 69911, 0.000091, 165,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema Solar'),
     N'Planeta mas grande del Sistema Solar'),

    ((SELECT id FROM tipos_objeto WHERE nombre=N'Planeta'),
     N'Saturno', 5.683e26, 58232, 0.000167, 134,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema Solar'),
     N'Famoso por sus anillos'),

    ((SELECT id FROM tipos_objeto WHERE nombre=N'Planeta'),
     N'Urano', 8.681e25, 25362, 0.000303, 76,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema Solar'),
     N'Gigante de hielo'),

    ((SELECT id FROM tipos_objeto WHERE nombre=N'Planeta'),
     N'Neptuno', 1.024e26, 24622, 0.000475, 72,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema Solar'),
     N'Planeta mas lejano del Sistema Solar');
GO

-- Exoplanetas de TRAPPIST-1 (muestra)
INSERT INTO objetos_astronomicos
    (tipo_id, nombre, masa_kg, radio_km, distancia_tierra_al, temperatura_k, sistema_id, descripcion)
VALUES
    ((SELECT id FROM tipos_objeto WHERE nombre=N'Exoplaneta'),
     N'TRAPPIST-1b', 5.265e24, 7012, 40.66, 391,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema TRAPPIST-1'),
     N'Exoplaneta rocoso'),

    ((SELECT id FROM tipos_objeto WHERE nombre=N'Exoplaneta'),
     N'TRAPPIST-1e', 4.117e24, 5918, 40.66, 251,
     (SELECT id FROM sistemas_planetarios WHERE nombre=N'Sistema TRAPPIST-1'),
     N'Posible zona habitable');
GO

-- ---------------------------------------------------------------------
-- Relaciones (aristas del grafo)
-- ---------------------------------------------------------------------
INSERT INTO relaciones (origen_id, destino_id, tipo_relacion, distancia_al, peso) VALUES
    ((SELECT id FROM objetos_astronomicos WHERE nombre=N'Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre=N'Mercurio'),        N'orbita',   0.00000915,   0.00000915),

    ((SELECT id FROM objetos_astronomicos WHERE nombre=N'Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre=N'Venus'),           N'orbita',   0.00000423,   0.00000423),

    ((SELECT id FROM objetos_astronomicos WHERE nombre=N'Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre=N'Tierra'),          N'orbita',   0.0000158,    0.0000158),

    ((SELECT id FROM objetos_astronomicos WHERE nombre=N'Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre=N'Marte'),           N'orbita',   0.0000402,    0.0000402),

    ((SELECT id FROM objetos_astronomicos WHERE nombre=N'Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre=N'Proxima Centauri'),N'cercania', 4.2465,       4.2465),

    ((SELECT id FROM objetos_astronomicos WHERE nombre=N'Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre=N'TRAPPIST-1'),      N'cercania', 40.66,        40.66),

    ((SELECT id FROM objetos_astronomicos WHERE nombre=N'TRAPPIST-1'),
     (SELECT id FROM objetos_astronomicos WHERE nombre=N'TRAPPIST-1b'),     N'orbita',   0.0000000017, 0.0000000017),

    ((SELECT id FROM objetos_astronomicos WHERE nombre=N'TRAPPIST-1'),
     (SELECT id FROM objetos_astronomicos WHERE nombre=N'TRAPPIST-1e'),     N'orbita',   0.0000000046, 0.0000000046);
GO

-- =====================================================================
-- FIN del script de datos iniciales
-- =====================================================================
