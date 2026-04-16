-- =====================================================================
-- Script de datos iniciales - Proyecto Astronomía Programación III
-- Ejecutar DESPUÉS de schema.sql
-- =====================================================================

-- ---------------------------------------------------------------------
-- Tipos de objeto
-- ---------------------------------------------------------------------
INSERT INTO tipos_objeto (nombre, descripcion) VALUES
    ('Planeta',        'Cuerpo celeste que orbita una estrella'),
    ('Estrella',       'Esfera de plasma unida por gravedad propia'),
    ('Galaxia',        'Sistema de estrellas, gas y polvo interestelar'),
    ('Agujero Negro',  'Región del espacio-tiempo con gravedad extrema'),
    ('Exoplaneta',     'Planeta fuera del Sistema Solar'),
    ('Satelite',       'Cuerpo que orbita un planeta'),
    ('Asteroide',      'Cuerpo rocoso menor del sistema solar');

-- ---------------------------------------------------------------------
-- Constelaciones (muestra)
-- ---------------------------------------------------------------------
INSERT INTO constelaciones (nombre, abreviatura, descripcion) VALUES
    ('Orion',          'Ori', 'Constelacion prominente del cielo de invierno'),
    ('Osa Mayor',      'UMa', 'Contiene el Carro o Cacerola'),
    ('Canis Major',    'CMa', 'Contiene a Sirio, la estrella mas brillante'),
    ('Centauro',       'Cen', 'Contiene al sistema Alpha Centauri'),
    ('Casiopea',       'Cas', 'Forma de W caracteristica');

-- ---------------------------------------------------------------------
-- Sistemas planetarios (sin estrella central aun, se actualiza despues)
-- ---------------------------------------------------------------------
INSERT INTO sistemas_planetarios (nombre, descripcion) VALUES
    ('Sistema Solar',         'Sistema donde se encuentra la Tierra'),
    ('Alpha Centauri',        'Sistema triple, el mas cercano al Sol'),
    ('Sistema TRAPPIST-1',    'Sistema con 7 exoplanetas terrestres');

-- ---------------------------------------------------------------------
-- Objetos astronomicos (muestra)
-- ---------------------------------------------------------------------
-- Estrellas centrales primero
INSERT INTO objetos_astronomicos
    (tipo_id, nombre, masa_kg, radio_km, distancia_tierra_al, temperatura_k, luminosidad, sistema_id, descripcion)
VALUES
    ((SELECT id FROM tipos_objeto WHERE nombre='Estrella'),
     'Sol', 1.989e30, 696340, 0.0000158, 5778, 1.0,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema Solar'),
     'Estrella del Sistema Solar'),

    ((SELECT id FROM tipos_objeto WHERE nombre='Estrella'),
     'Proxima Centauri', 2.446e29, 107000, 4.2465, 3042, 0.0017,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Alpha Centauri'),
     'Estrella mas cercana al Sol'),

    ((SELECT id FROM tipos_objeto WHERE nombre='Estrella'),
     'TRAPPIST-1', 1.79e29, 84000, 40.66, 2566, 0.000553,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema TRAPPIST-1'),
     'Enana roja ultrafria con 7 exoplanetas');

-- Actualizar sistemas con estrella central
UPDATE sistemas_planetarios SET estrella_central_id = (SELECT id FROM objetos_astronomicos WHERE nombre='Sol') WHERE nombre='Sistema Solar';
UPDATE sistemas_planetarios SET estrella_central_id = (SELECT id FROM objetos_astronomicos WHERE nombre='Proxima Centauri') WHERE nombre='Alpha Centauri';
UPDATE sistemas_planetarios SET estrella_central_id = (SELECT id FROM objetos_astronomicos WHERE nombre='TRAPPIST-1') WHERE nombre='Sistema TRAPPIST-1';

-- Planetas del Sistema Solar
INSERT INTO objetos_astronomicos
    (tipo_id, nombre, masa_kg, radio_km, distancia_tierra_al, temperatura_k, sistema_id, descripcion)
VALUES
    ((SELECT id FROM tipos_objeto WHERE nombre='Planeta'),
     'Mercurio', 3.3011e23, 2439.7, 0.00000915, 440,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema Solar'),
     'Planeta mas cercano al Sol'),

    ((SELECT id FROM tipos_objeto WHERE nombre='Planeta'),
     'Venus', 4.8675e24, 6051.8, 0.00000423, 737,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema Solar'),
     'Planeta mas caliente del Sistema Solar'),

    ((SELECT id FROM tipos_objeto WHERE nombre='Planeta'),
     'Tierra', 5.972e24, 6371.0, 0.0, 288,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema Solar'),
     'Unico planeta conocido con vida'),

    ((SELECT id FROM tipos_objeto WHERE nombre='Planeta'),
     'Marte', 6.4171e23, 3389.5, 0.0000402, 210,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema Solar'),
     'Planeta rojo'),

    ((SELECT id FROM tipos_objeto WHERE nombre='Planeta'),
     'Jupiter', 1.898e27, 69911, 0.000091, 165,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema Solar'),
     'Planeta mas grande del Sistema Solar'),

    ((SELECT id FROM tipos_objeto WHERE nombre='Planeta'),
     'Saturno', 5.683e26, 58232, 0.000167, 134,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema Solar'),
     'Famoso por sus anillos'),

    ((SELECT id FROM tipos_objeto WHERE nombre='Planeta'),
     'Urano', 8.681e25, 25362, 0.000303, 76,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema Solar'),
     'Gigante de hielo'),

    ((SELECT id FROM tipos_objeto WHERE nombre='Planeta'),
     'Neptuno', 1.024e26, 24622, 0.000475, 72,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema Solar'),
     'Planeta mas lejano del Sistema Solar');

-- Exoplanetas de TRAPPIST-1 (muestra)
INSERT INTO objetos_astronomicos
    (tipo_id, nombre, masa_kg, radio_km, distancia_tierra_al, temperatura_k, sistema_id, descripcion)
VALUES
    ((SELECT id FROM tipos_objeto WHERE nombre='Exoplaneta'),
     'TRAPPIST-1b', 5.265e24, 7012, 40.66, 391,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema TRAPPIST-1'),
     'Exoplaneta rocoso'),

    ((SELECT id FROM tipos_objeto WHERE nombre='Exoplaneta'),
     'TRAPPIST-1e', 4.117e24, 5918, 40.66, 251,
     (SELECT id FROM sistemas_planetarios WHERE nombre='Sistema TRAPPIST-1'),
     'Posible zona habitable');

-- ---------------------------------------------------------------------
-- Relaciones (aristas del grafo)
-- ---------------------------------------------------------------------
INSERT INTO relaciones (origen_id, destino_id, tipo_relacion, distancia_al, peso) VALUES
    ((SELECT id FROM objetos_astronomicos WHERE nombre='Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre='Mercurio'), 'orbita', 0.00000915, 0.00000915),

    ((SELECT id FROM objetos_astronomicos WHERE nombre='Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre='Venus'), 'orbita', 0.00000423, 0.00000423),

    ((SELECT id FROM objetos_astronomicos WHERE nombre='Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre='Tierra'), 'orbita', 0.0000158, 0.0000158),

    ((SELECT id FROM objetos_astronomicos WHERE nombre='Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre='Marte'), 'orbita', 0.0000402, 0.0000402),

    ((SELECT id FROM objetos_astronomicos WHERE nombre='Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre='Proxima Centauri'), 'cercania', 4.2465, 4.2465),

    ((SELECT id FROM objetos_astronomicos WHERE nombre='Sol'),
     (SELECT id FROM objetos_astronomicos WHERE nombre='TRAPPIST-1'), 'cercania', 40.66, 40.66),

    ((SELECT id FROM objetos_astronomicos WHERE nombre='TRAPPIST-1'),
     (SELECT id FROM objetos_astronomicos WHERE nombre='TRAPPIST-1b'), 'orbita', 0.0000000017, 0.0000000017),

    ((SELECT id FROM objetos_astronomicos WHERE nombre='TRAPPIST-1'),
     (SELECT id FROM objetos_astronomicos WHERE nombre='TRAPPIST-1e'), 'orbita', 0.0000000046, 0.0000000046);

-- =====================================================================
-- FIN del script de datos iniciales
-- =====================================================================
