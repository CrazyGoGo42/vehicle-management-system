<?php
require_once 'utils/cors.php';
require_once 'utils/response.php';

// Simple routing for API endpoints
$request_uri = $_SERVER['REQUEST_URI'];
$path = parse_url($request_uri, PHP_URL_PATH);

// Remove query string and trailing slashes
$path = rtrim($path, '/');

// Route to appropriate API endpoint
if (strpos($path, '/api/vehicles') === 0 || strpos($path, '/backend-php/api/vehicles') === 0) {
    require_once 'api/vehicles.php';
} else {
    // API documentation or welcome message
    ApiResponse::success([
        'message' => 'Vehicle Manager PHP API',
        'version' => '1.0.0',
        'endpoints' => [
            'GET /api/vehicles' => 'Get all vehicles',
            'GET /api/vehicles/{id}' => 'Get vehicle by ID',
            'GET /api/vehicles?search={term}' => 'Search vehicles',
            'POST /api/vehicles' => 'Create new vehicle',
            'PUT /api/vehicles/{id}' => 'Update vehicle',
            'DELETE /api/vehicles/{id}' => 'Delete vehicle'
        ]
    ]);
}
?>