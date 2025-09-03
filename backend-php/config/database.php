<?php
class Database {
    private $database = 'vehiclemanager.db';
    private $connection;

    public function getConnection() {
        $this->connection = null;
        
        try {
            $dsn = "sqlite:" . __DIR__ . "/../" . $this->database;
            $this->connection = new PDO($dsn);
            $this->connection->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
            $this->connection->setAttribute(PDO::ATTR_DEFAULT_FETCH_MODE, PDO::FETCH_ASSOC);
            
            // Create table if not exists
            $this->createTable();
            
        } catch(PDOException $exception) {
            error_log("Database connection error: " . $exception->getMessage());
            throw new Exception("Database connection failed");
        }
        
        return $this->connection;
    }
    
    private function createTable() {
        $sql = "CREATE TABLE IF NOT EXISTS vehicles (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Marke TEXT NOT NULL,
            Modell TEXT NOT NULL,
            Baujahr INTEGER NOT NULL,
            Kaufpreis DECIMAL(10,2) NOT NULL,
            Leistung INTEGER NOT NULL,
            Kilometerstand INTEGER NOT NULL,
            Kraftstoff TEXT NOT NULL,
            Getriebe TEXT NOT NULL,
            Farbe TEXT NOT NULL,
            Kaufdatum DATE NOT NULL,
            Fahrzeugtyp TEXT NOT NULL,
            Zusatzausstattung TEXT,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        )";
        
        $this->connection->exec($sql);
    }
}
?>