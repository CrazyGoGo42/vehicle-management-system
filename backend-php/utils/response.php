<?php
class ApiResponse {
    public static function success($data = null, $message = "Success", $code = 200) {
        http_response_code($code);
        echo json_encode([
            'success' => true,
            'message' => $message,
            'data' => $data
        ]);
        exit();
    }

    public static function error($message = "Error", $code = 400, $details = null) {
        http_response_code($code);
        echo json_encode([
            'success' => false,
            'message' => $message,
            'details' => $details
        ]);
        exit();
    }

    public static function notFound($message = "Resource not found") {
        self::error($message, 404);
    }

    public static function serverError($message = "Internal server error") {
        self::error($message, 500);
    }

    public static function badRequest($message = "Bad request") {
        self::error($message, 400);
    }
}
?>