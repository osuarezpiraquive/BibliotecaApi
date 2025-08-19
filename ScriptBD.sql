CREATE DATABASE IF NOT EXISTS BibliotecaDB;
USE BibliotecaDB;

-- Tabla Categorias
CREATE TABLE Categorias (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Nombre VARCHAR(100) NOT NULL
);

-- Tabla Libros
CREATE TABLE Libros (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Titulo VARCHAR(200) NOT NULL,
    Autor VARCHAR(150) NOT NULL,
    ISBN VARCHAR(50) NOT NULL,
    AnioPublicacion INT NOT NULL,
    Disponible BOOLEAN NOT NULL DEFAULT TRUE,
    CategoriaId INT,
    FOREIGN KEY (CategoriaId) REFERENCES Categorias(Id) ON DELETE SET NULL
);


CREATE TABLE LogMetrics (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Timestamp DATETIME NOT NULL,
    Level VARCHAR(20) NOT NULL,         -- INFO, ERROR, WARN
    Message TEXT NOT NULL,
    MetricType VARCHAR(50) NULL,        -- Tipo de métrica (ej: "UserLogin", "MemoryUsage")
    MetricValue VARCHAR(100) NULL,      -- Valor asociado (ej: "user123", "85%")
    CreatedAt TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

-- Datos de Prueba
INSERT INTO Categorias (Nombre) VALUES ('Novela'), ('Historia'), ('Tecnología');

INSERT INTO Libros (Titulo, Autor, ISBN, AnioPublicacion, Disponible, CategoriaId)
VALUES 
('Cien Años de Soledad', 'Gabriel García Márquez', '978-84-376-0494-7', 1967, TRUE, 1),
('Breve Historia del Tiempo', 'Stephen Hawking', '978-0553380163', 1988, TRUE, 2),
('Aprendiendo C#', 'John Doe', '123-4567890123', 2020, TRUE, 3);