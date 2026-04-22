-- =====================================================================
-- crear-bd.sql — Ejecutar PRIMERO en SSMS conectado a master
-- Crea la base de datos AstronomiaDB si no existe
-- =====================================================================

USE master;
GO

IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'AstronomiaDB')
BEGIN
    CREATE DATABASE AstronomiaDB;
    PRINT 'Base de datos AstronomiaDB creada.';
END
ELSE
BEGIN
    PRINT 'AstronomiaDB ya existe — no se crea de nuevo.';
END
GO

USE AstronomiaDB;
GO

PRINT 'Conectado a AstronomiaDB. Ahora ejecutar schema.sql';
GO
