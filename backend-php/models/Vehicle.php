<?php
class Vehicle {
    private $connection;
    private $table_name = "vehicles";

    // Vehicle properties
    public $Id;
    public $Marke;
    public $Modell;
    public $Baujahr;
    public $Kaufpreis;
    public $Leistung;
    public $Kilometerstand;
    public $Kraftstoff;
    public $Getriebe;
    public $Farbe;
    public $Kaufdatum;
    public $Fahrzeugtyp;
    public $Zusatzausstattung;
    public $created_at;
    public $updated_at;

    public function __construct($db) {
        $this->connection = $db;
    }

    // Get all vehicles
    public function getAllVehicles() {
        $query = "SELECT * FROM " . $this->table_name . " ORDER BY Id";
        $stmt = $this->connection->prepare($query);
        $stmt->execute();
        return $stmt->fetchAll(PDO::FETCH_ASSOC);
    }

    // Get vehicle by ID
    public function getVehicleById($id) {
        $query = "SELECT * FROM " . $this->table_name . " WHERE Id = ? LIMIT 1";
        $stmt = $this->connection->prepare($query);
        $stmt->bindParam(1, $id);
        $stmt->execute();
        return $stmt->fetch(PDO::FETCH_ASSOC);
    }

    // Search vehicles
    public function searchVehicles($searchText) {
        $query = "SELECT * FROM " . $this->table_name . " 
                  WHERE Marke LIKE ? OR Modell LIKE ? 
                  ORDER BY Id";
        $stmt = $this->connection->prepare($query);
        $searchTerm = "%" . $searchText . "%";
        $stmt->bindParam(1, $searchTerm);
        $stmt->bindParam(2, $searchTerm);
        $stmt->execute();
        return $stmt->fetchAll(PDO::FETCH_ASSOC);
    }

    // Create vehicle
    public function createVehicle($data) {
        $query = "INSERT INTO " . $this->table_name . " 
                  (Marke, Modell, Baujahr, Kaufpreis, Leistung, Kilometerstand, 
                   Kraftstoff, Getriebe, Farbe, Kaufdatum, Fahrzeugtyp, Zusatzausstattung) 
                  VALUES (:marke, :modell, :baujahr, :kaufpreis, :leistung, :kilometerstand,
                          :kraftstoff, :getriebe, :farbe, :kaufdatum, :fahrzeugtyp, :zusatzausstattung)";

        $stmt = $this->connection->prepare($query);

        // Bind parameters
        $stmt->bindParam(':marke', $data['Marke']);
        $stmt->bindParam(':modell', $data['Modell']);
        $stmt->bindParam(':baujahr', $data['Baujahr']);
        $stmt->bindParam(':kaufpreis', $data['Kaufpreis']);
        $stmt->bindParam(':leistung', $data['Leistung']);
        $stmt->bindParam(':kilometerstand', $data['Kilometerstand']);
        $stmt->bindParam(':kraftstoff', $data['Kraftstoff']);
        $stmt->bindParam(':getriebe', $data['Getriebe']);
        $stmt->bindParam(':farbe', $data['Farbe']);
        $stmt->bindParam(':kaufdatum', $data['Kaufdatum']);
        $stmt->bindParam(':fahrzeugtyp', $data['Fahrzeugtyp']);
        $stmt->bindParam(':zusatzausstattung', $data['Zusatzausstattung']);

        if ($stmt->execute()) {
            return $this->connection->lastInsertId();
        }
        return false;
    }

    // Update vehicle
    public function updateVehicle($id, $data) {
        $query = "UPDATE " . $this->table_name . " 
                  SET Marke = :marke, Modell = :modell, Baujahr = :baujahr, 
                      Kaufpreis = :kaufpreis, Leistung = :leistung, Kilometerstand = :kilometerstand,
                      Kraftstoff = :kraftstoff, Getriebe = :getriebe, Farbe = :farbe,
                      Kaufdatum = :kaufdatum, Fahrzeugtyp = :fahrzeugtyp, 
                      Zusatzausstattung = :zusatzausstattung, updated_at = CURRENT_TIMESTAMP
                  WHERE Id = :id";

        $stmt = $this->connection->prepare($query);

        // Bind parameters
        $stmt->bindParam(':id', $id);
        $stmt->bindParam(':marke', $data['Marke']);
        $stmt->bindParam(':modell', $data['Modell']);
        $stmt->bindParam(':baujahr', $data['Baujahr']);
        $stmt->bindParam(':kaufpreis', $data['Kaufpreis']);
        $stmt->bindParam(':leistung', $data['Leistung']);
        $stmt->bindParam(':kilometerstand', $data['Kilometerstand']);
        $stmt->bindParam(':kraftstoff', $data['Kraftstoff']);
        $stmt->bindParam(':getriebe', $data['Getriebe']);
        $stmt->bindParam(':farbe', $data['Farbe']);
        $stmt->bindParam(':kaufdatum', $data['Kaufdatum']);
        $stmt->bindParam(':fahrzeugtyp', $data['Fahrzeugtyp']);
        $stmt->bindParam(':zusatzausstattung', $data['Zusatzausstattung']);

        return $stmt->execute();
    }

    // Delete vehicle
    public function deleteVehicle($id) {
        $query = "DELETE FROM " . $this->table_name . " WHERE Id = ?";
        $stmt = $this->connection->prepare($query);
        $stmt->bindParam(1, $id);
        return $stmt->execute();
    }

    // Calculate current value (business logic from C#)
    public function calculateCurrentValue($kaufpreis, $baujahr) {
        $currentYear = date('Y');
        $age = $currentYear - $baujahr;
        
        if ($age <= 0) {
            return $kaufpreis; // New car, no depreciation
        }
        
        // Depreciation: 19% first year, then 5.7% per year
        $depreciationRate = 0.19 + ($age - 1) * 0.057;
        $currentValue = $kaufpreis * (1 - $depreciationRate);
        
        // Never go below 10% of purchase price
        $minimumValue = $kaufpreis * 0.1;
        
        return max($currentValue, $minimumValue);
    }
}
?>