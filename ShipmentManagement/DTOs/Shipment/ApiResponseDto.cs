namespace ShipmentManagement.DTOs.Shipment
{
    public class ApiResponseDto<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public int Count { get; set; }
    }
}