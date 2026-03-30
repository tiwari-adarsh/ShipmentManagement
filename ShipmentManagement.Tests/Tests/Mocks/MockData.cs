using ShipmentManagement.Models;
using ShipmentManagement.Models.Enums;

namespace ShipmentManagement.Tests.Mocks
{
    public static class MockData
    {
        // Ships
        public static List<Ship> GetShips() => new()
        {
            new Ship
            {
                Id          = 1,
                ShipCode    = "S001",
                ShipName    = "MV Titan",
                ImoNumber   = "9876543",
                CapacityMT  = 52000,
                ShipType    = "Bulk Carrier",
                FlagCountry = "India",
                YearBuilt   = 2018,
                Status      = ShipStatus.Active,
                CreatedAt   = DateTime.Now,
            },
            new Ship
            {
                Id          = 2,
                ShipCode    = "S002",
                ShipName    = "MV Neptune",
                ImoNumber   = "9765432",
                CapacityMT  = 38000,
                ShipType    = "Container",
                FlagCountry = "India",
                YearBuilt   = 2020,
                Status      = ShipStatus.Active,
                CreatedAt   = DateTime.Now,
            },
            new Ship
            {
                Id          = 3,
                ShipCode    = "S003",
                ShipName    = "MV Horizon",
                ImoNumber   = "9654321",
                CapacityMT  = 65000,
                ShipType    = "Tanker",
                FlagCountry = "Singapore",
                YearBuilt   = 2016,
                Status      = ShipStatus.Maintenance, // ← Must be this exactly
                CreatedAt   = DateTime.Now,
            },
        };

        //  Customers 
        public static List<Customer> GetCustomers() => new()
        {
            new Customer
            {
                Id           = 1,
                CustomerCode = "C001",
                CustomerName = "Rajesh Mehta",
                CompanyName  = "Reliance Industries",
                Email        = "r.mehta@reliance.com",
                Phone        = "+91 98765 43210",
                City         = "Mumbai",
                CreatedAt    = DateTime.Now,
            },
            new Customer
            {
                Id           = 2,
                CustomerCode = "C002",
                CustomerName = "Anita Sharma",
                CompanyName  = "Tata Steel",
                Email        = "anita.s@tatasteel.com",
                Phone        = "+91 87654 32109",
                City         = "Jamshedpur",
                CreatedAt    = DateTime.Now,
            },
        };

        // Ports 
        public static List<Port> GetPorts() => new()
        {
            new Port { Id=1, PortCode="INBOM", PortName="Mumbai Port",     Country="India", City="Mumbai",    IsActive=true },
            new Port { Id=2, PortCode="AEJEA", PortName="Jebel Ali Dubai", Country="UAE",   City="Dubai",     IsActive=true },
            new Port { Id=3, PortCode="SGSIN", PortName="Singapore PSA",   Country="Singapore",City="Singapore",IsActive=true },
        };

        // Shipments
        public static List<Shipment> GetShipments() => new()
        {
            new Shipment
            {
                Id              = 1,
                ShipmentCode    = "SHP-2024-0001",
                BookingDate     = DateTime.Now.AddDays(-10),
                ShipmentType    = ShipmentType.Export,
                CargoType       = CargoType.GeneralCargo,
                WeightMT        = 2500,
                ShipId          = 1,
                CustomerId      = 1,
                SourcePort      = "Mumbai Port",
                DestinationPort = "Jebel Ali Dubai",
                DepartureDate   = DateTime.Now.AddDays(-8),
                ArrivalDate     = DateTime.Now.AddDays(5),  // Future = not overdue
                Status          = ShipmentStatus.InTransit,
                CreatedAt       = DateTime.Now.AddDays(-10),
                Ship            = GetShips()[0],
                Customer        = GetCustomers()[0],
            },
            new Shipment
            {
                Id              = 2,
                ShipmentCode    = "SHP-2024-0002",
                BookingDate     = DateTime.Now.AddDays(-20),
                ShipmentType    = ShipmentType.Import,
                CargoType       = CargoType.BulkCargo,
                WeightMT        = 15000,
                ShipId          = 2,
                CustomerId      = 2,
                SourcePort      = "Jebel Ali Dubai",
                DestinationPort = "Mumbai Port",
                DepartureDate   = DateTime.Now.AddDays(-15),
                ArrivalDate     = DateTime.Now.AddDays(-2), // Past = OVERDUE
                Status          = ShipmentStatus.InTransit,
                CreatedAt       = DateTime.Now.AddDays(-20),
                Ship            = GetShips()[1],
                Customer        = GetCustomers()[1],
            },
            new Shipment
            {
                Id              = 3,
                ShipmentCode    = "SHP-2024-0003",
                BookingDate     = DateTime.Now.AddDays(-30),
                ShipmentType    = ShipmentType.Export,
                CargoType       = CargoType.Container,
                WeightMT        = 5000,
                ShipId          = 1,
                CustomerId      = 1,
                SourcePort      = "Mumbai Port",
                DestinationPort = "Singapore PSA",
                DepartureDate   = DateTime.Now.AddDays(-25),
                ArrivalDate     = DateTime.Now.AddDays(-10),
                Status          = ShipmentStatus.Delivered,  // Already delivered
                CreatedAt       = DateTime.Now.AddDays(-30),
                Ship            = GetShips()[0],
                Customer        = GetCustomers()[0],
            },
        };

        //  Overdue Shipments 
        public static List<Shipment> GetOverdueShipments() =>
            GetShipments().Where(s =>
                s.ArrivalDate.HasValue &&
                s.ArrivalDate.Value < DateTime.Now &&
                s.Status != ShipmentStatus.Delivered &&
                s.Status != ShipmentStatus.Delayed
            ).ToList();
    }
}