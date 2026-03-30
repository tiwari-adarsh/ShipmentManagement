namespace ShipmentManagement.DTOs.Shipment
{
    public class ShipmentCreateDto
    {
        public DateTime BookingDate { get; set; }
        public string ShipmentType { get; set; } = string.Empty;
        public string CargoType { get; set; } = string.Empty;
        public decimal WeightMT { get; set; }
        public string? ContainerNumber { get; set; }
        public int ShipId { get; set; }
        public int CustomerId { get; set; }
        public string SourcePort { get; set; } = string.Empty;
        public string DestinationPort { get; set; } = string.Empty;
        public DateTime DepartureDate { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string? SpecialInstructions { get; set; }
    }
}