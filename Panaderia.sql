USE master;
GO

-- Eliminar la base de datos si existe
IF EXISTS(SELECT name FROM sys.databases WHERE name = 'Panaderia')
BEGIN
    ALTER DATABASE Panaderia SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE Panaderia;
    PRINT '🗑️ Base de datos Panaderia eliminada';
END
GO

-- Crear la base de datos
CREATE DATABASE Panaderia;
PRINT '✅ Base de datos Panaderia creada';
GO

USE Panaderia;
GO

-- Tabla de Categorías
CREATE TABLE Categorias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL
);
PRINT '✅ Tabla Categorias creada';
GO

-- Tabla de Productos
CREATE TABLE Productos (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Precio DECIMAL(10,2) NOT NULL CHECK (Precio >= 0),
    Stock INT NOT NULL CHECK (Stock >= 0),
    CategoriaId INT NOT NULL,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,
    Descripcion NVARCHAR(500) NULL,
    CONSTRAINT FK_Productos_Categorias FOREIGN KEY (CategoriaId)
        REFERENCES Categorias(Id)
);
PRINT '✅ Tabla Productos creada';
GO

-- Tabla de Ventas
CREATE TABLE Ventas (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ProductoId INT NOT NULL,
    Cantidad INT NOT NULL CHECK (Cantidad > 0),
    FechaVenta DATETIME DEFAULT GETDATE(),
    Cliente NVARCHAR(100) NOT NULL,
    Vendedor NVARCHAR(100) NOT NULL,
    Total DECIMAL(10,2) NOT NULL,
    Observaciones NVARCHAR(300) NULL,
    CONSTRAINT FK_Ventas_Productos FOREIGN KEY (ProductoId)
        REFERENCES Productos(Id)
);
PRINT '✅ Tabla Ventas creada';
GO

-- Insertar SOLO categorías (sin productos)
INSERT INTO Categorias (Nombre) VALUES 
('Pan Dulce'), 
('Pan Salado'), 
('Bebidas'), 
('Repostería'),
('Galletas'),
('Otros');
PRINT '✅ Categorías insertadas';
GO

-- Crear índices para mejor rendimiento
CREATE INDEX IX_Productos_CategoriaId ON Productos(CategoriaId);
CREATE INDEX IX_Productos_Activo ON Productos(Activo);
CREATE INDEX IX_Ventas_FechaVenta ON Ventas(FechaVenta);
CREATE INDEX IX_Ventas_Vendedor ON Ventas(Vendedor);
PRINT '✅ Índices creados';
GO

-- Verificar que todo se creó correctamente
SELECT '🎉 BASE DE DATOS CREADA EXITOSAMENTE' AS Mensaje;
PRINT ' ';
SELECT '📊 RESUMEN:' AS Info;
PRINT ' ';

SELECT 'Categorías disponibles:' AS Tabla;
SELECT Id, Nombre FROM Categorias;
PRINT ' ';

SELECT 'Productos:' AS Tabla;  
SELECT 'La base está vacía - Puedes agregar productos desde la aplicación' AS Estado;
PRINT ' ';

SELECT 'Ventas:' AS Tabla;
SELECT 'No hay ventas registradas aún' AS Estado;
GO