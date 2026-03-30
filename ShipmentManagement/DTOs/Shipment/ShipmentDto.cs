namespace ShipmentManagement.DTOs.Shipment
{
    public class ShipmentDto
    {
        public int Id { get; set; }
        public string ShipmentCode { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string CompanyName { get; set; } = string.Empty;
        public string ShipName { get; set; } = string.Empty;
        public string ShipCode { get; set; } = string.Empty;
        public string ShipmentType { get; set; } = string.Empty;
        public string CargoType { get; set; } = string.Empty;
        public decimal WeightMT { get; set; }
        public string SourcePort { get; set; } = string.Empty;
        public string DestinationPort { get; set; } = string.Empty;
        public DateTime BookingDate { get; set; }
        public DateTime DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public DateTime? ActualArrivalDate { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ContainerNumber { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}