-- Database Setup für phpMyAdmin
-- Diese SQL-Statements in phpMyAdmin ausführen

CREATE DATABASE IF NOT EXISTS vehiclemanager 
CHARACTER SET utf8mb4 COLLATE utf8mb4_unicode_ci;

USE vehiclemanager;

CREATE TABLE vehicles (
    Id INT AUTO_INCREMENT PRIMARY KEY,
    Marke VARCHAR(50) NOT NULL,
    Modell VARCHAR(50) NOT NULL,
    Baujahr INT NOT NULL,
    Kaufpreis DECIMAL(10,2) NOT NULL,
    Leistung INT NOT NULL COMMENT 'Leistung in PS',
    Kilometerstand INT NOT NULL,
    Kraftstoff VARCHAR(20) NOT NULL,
    Getriebe VARCHAR(20) NOT NULL,
    Farbe VARCHAR(30) NOT NULL,
    Kaufdatum DATE NOT NULL,
    Fahrzeugtyp VARCHAR(30) NOT NULL,
    Zusatzausstattung TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Beispiel-Datensätze
INSERT INTO vehicles (Marke, Modell, Baujahr, Kaufpreis, Leistung, Kilometerstand, Kraftstoff, Getriebe, Farbe, Kaufdatum, Fahrzeugtyp, Zusatzausstattung)
VALUES 
    ('Volkswagen', 'Golf', 2020, 25000.00, 150, 45000, 'Benzin', 'Manuell', 'Blau', '2020-03-15', 'PKW', 'Klimaanlage, Navigationssystem'),
    ('BMW', 'X3', 2019, 45000.00, 190, 67000, 'Diesel', 'Automatik', 'Schwarz', '2019-07-22', 'SUV', 'Ledersitze, Panoramadach'),
    ('Mercedes-Benz', 'A-Klasse', 2021, 32000.00, 163, 23000, 'Benzin', 'Automatik', 'Weiß', '2021-01-10', 'PKW', 'MBUX, LED-Scheinwerfer');