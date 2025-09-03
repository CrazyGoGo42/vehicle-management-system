<?php
try {
    $dsn = "mysql:host=127.0.0.1;port=3307;dbname=vehiclemanager;charset=utf8mb4";
    $username = 'root';
    $password = '1234';
    
    echo "Attempting connection with:\n";
    echo "DSN: $dsn\n";
    echo "User: $username\n";
    
    $pdo = new PDO($dsn, $username, $password);
    $pdo->setAttribute(PDO::ATTR_ERRMODE, PDO::ERRMODE_EXCEPTION);
    
    echo "✅ Connection successful!\n";
    
    // Test query
    $stmt = $pdo->query("SELECT COUNT(*) as count FROM vehicles");
    $result = $stmt->fetch(PDO::FETCH_ASSOC);
    echo "Vehicles count: " . $result['count'] . "\n";
    
} catch (PDOException $e) {
    echo "❌ Connection failed: " . $e->getMessage() . "\n";
    echo "Error code: " . $e->getCode() . "\n";
}
?>