namespace ShipmentManagement.Models
{
    public class DashboardStat
    {
        public int TotalShipments { get; set; }
        public int Upcoming { get; set; }
        public int Ongoing { get; set; }
        public int Delivered { get; set; }
        public int Delayed { get; set; }
    }
}
