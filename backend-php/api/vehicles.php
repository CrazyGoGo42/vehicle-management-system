<?php
require_once __DIR__ . '/../config/database.php';
require_once __DIR__ . '/../models/Vehicle.php';
require_once __DIR__ . '/../utils/cors.php';
require_once __DIR__ . '/../utils/response.php';

try {
    // Initialize database connection
    $database = new Database();
    $db = $database->getConnection();
    $vehicle = new Vehicle($db);

    // Get request method
    $method = $_SERVER['REQUEST_METHOD'];
    
    // Parse the URL to get the ID if provided
    $request_uri = $_SERVER['REQUEST_URI'];
    $path = parse_url($request_uri, PHP_URL_PATH);
    $path_parts = explode('/', trim($path, '/'));
    
    // Extract ID from URL if present (look for numeric part after vehicles)
    $vehicle_id = null;
    $vehicles_index = array_search('vehicles', $path_parts);
    if ($vehicles_index !== false && isset($path_parts[$vehicles_index + 1]) && is_numeric($path_parts[$vehicles_index + 1])) {
        $vehicle_id = (int)$path_parts[$vehicles_index + 1];
    }

    switch ($method) {
        case 'GET':
            if (isset($_GET['search']) && !empty($_GET['search'])) {
                // Search vehicles
                $searchText = $_GET['search'];
                $vehicles = $vehicle->searchVehicles($searchText);
                
                // Add calculated current value to each vehicle
                foreach ($vehicles as &$v) {
                    $v['AktuellerWert'] = $vehicle->calculateCurrentValue($v['Kaufpreis'], $v['Baujahr']);
                }
                
                ApiResponse::success($vehicles, "Search completed successfully");
            } elseif ($vehicle_id) {
                // Get single vehicle by ID
                $vehicleData = $vehicle->getVehicleById($vehicle_id);
                if ($vehicleData) {
                    $vehicleData['AktuellerWert'] = $vehicle->calculateCurrentValue($vehicleData['Kaufpreis'], $vehicleData['Baujahr']);
                    ApiResponse::success($vehicleData, "Vehicle retrieved successfully");
                } else {
                    ApiResponse::notFound("Vehicle not found");
                }
            } else {
                // Get all vehicles
                $vehicles = $vehicle->getAllVehicles();
                
                // Add calculated current value to each vehicle
                foreach ($vehicles as &$v) {
                    $v['AktuellerWert'] = $vehicle->calculateCurrentValue($v['Kaufpreis'], $v['Baujahr']);
                }
                
                ApiResponse::success($vehicles, "Vehicles retrieved successfully");
            }
            break;

        case 'POST':
            // Create new vehicle
            $input = json_decode(file_get_contents('php://input'), true);
            
            if (!$input) {
                ApiResponse::badRequest("Invalid JSON data");
            }
            
            // Validate required fields
            $required_fields = ['Marke', 'Modell', 'Baujahr', 'Kaufpreis', 'Leistung', 'Kilometerstand', 
                              'Kraftstoff', 'Getriebe', 'Farbe', 'Kaufdatum', 'Fahrzeugtyp'];
            
            foreach ($required_fields as $field) {
                if (!isset($input[$field]) || empty($input[$field])) {
                    ApiResponse::badRequest("Missing required field: " . $field);
                }
            }
            
            // Set default for optional fields
            if (!isset($input['Zusatzausstattung'])) {
                $input['Zusatzausstattung'] = '';
            }
            
            $new_id = $vehicle->createVehicle($input);
            if ($new_id) {
                $new_vehicle = $vehicle->getVehicleById($new_id);
                $new_vehicle['AktuellerWert'] = $vehicle->calculateCurrentValue($new_vehicle['Kaufpreis'], $new_vehicle['Baujahr']);
                ApiResponse::success($new_vehicle, "Vehicle created successfully", 201);
            } else {
                ApiResponse::serverError("Failed to create vehicle");
            }
            break;

        case 'PUT':
            // Update vehicle
            if (!$vehicle_id) {
                ApiResponse::badRequest("Vehicle ID is required for update");
            }
            
            $input = json_decode(file_get_contents('php://input'), true);
            
            if (!$input) {
                ApiResponse::badRequest("Invalid JSON data");
            }
            
            // Check if vehicle exists
            $existing_vehicle = $vehicle->getVehicleById($vehicle_id);
            if (!$existing_vehicle) {
                ApiResponse::notFound("Vehicle not found");
            }
            
            if ($vehicle->updateVehicle($vehicle_id, $input)) {
                $updated_vehicle = $vehicle->getVehicleById($vehicle_id);
                $updated_vehicle['AktuellerWert'] = $vehicle->calculateCurrentValue($updated_vehicle['Kaufpreis'], $updated_vehicle['Baujahr']);
                ApiResponse::success($updated_vehicle, "Vehicle updated successfully");
            } else {
                ApiResponse::serverError("Failed to update vehicle");
            }
            break;

        case 'DELETE':
            // Delete vehicle
            if (!$vehicle_id) {
                ApiResponse::badRequest("Vehicle ID is required for deletion");
            }
            
            // Check if vehicle exists
            $existing_vehicle = $vehicle->getVehicleById($vehicle_id);
            if (!$existing_vehicle) {
                ApiResponse::notFound("Vehicle not found");
            }
            
            if ($vehicle->deleteVehicle($vehicle_id)) {
                ApiResponse::success(null, "Vehicle deleted successfully");
            } else {
                ApiResponse::serverError("Failed to delete vehicle");
            }
            break;

        default:
            ApiResponse::error("Method not allowed", 405);
            break;
    }

} catch (Exception $e) {
    error_log("API Error: " . $e->getMessage());
    ApiResponse::serverError("An error occurred: " . $e->getMessage());
}
?>