using Microsoft.EntityFrameworkCore;
using ShipmentManagement.Models;

namespace ShipmentManagement.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSets 
        public DbSet<Ship> Ships { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<Shipment> Shipments { get; set; }
        public DbSet<ShipmentStatusLog> ShipmentStatusLogs { get; set; }
        public DbSet<ShipmentTracking> ShipmentTrackings { get; set; }
        public DbSet<User> Users { get; set; }

        public DbSet<LogHistory> LogHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //  Ship 
            modelBuilder.Entity<Ship>(entity =>
            {
                entity.HasIndex(s => s.ShipCode).IsUnique();
                entity.HasIndex(s => s.ImoNumber).IsUnique();
                entity.Property(s => s.CapacityMT).HasColumnType("decimal(10,2)");
            });

            //  Customer 
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasIndex(c => c.CustomerCode).IsUnique();
                entity.HasIndex(c => c.Email).IsUnique();
            });

            //  Port 
            modelBuilder.Entity<Port>(entity =>
            {
                entity.HasIndex(p => p.PortCode).IsUnique();
            });

            //  Shipment 
            modelBuilder.Entity<Shipment>(entity =>
            {
                entity.HasIndex(s => s.ShipmentCode).IsUnique();
                entity.Property(s => s.WeightMT).HasColumnType("decimal(10,2)");

                // Shipment → Ship (many-to-one)
                entity.HasOne(s => s.Ship)                     // - HasOne: Each tracking entry is linked to one Shipment
                      .WithMany(sh => sh.Shipments)           // - WithMany: A Shipment can have multiple tracking entries
                      .HasForeignKey(s => s.ShipId)          // - HasForeignKey: ShipmentId is used as the foreign key
                      .OnDelete(DeleteBehavior.Restrict);   // - OnDelete(Cascade): Deleting a Shipment will also delete its tracking records

                // Shipment → Customer (many-to-one)
                entity.HasOne(s => s.Customer)
                      .WithMany(c => c.Shipments)
                      .HasForeignKey(s => s.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            //  ShipmentStatusLog 
            modelBuilder.Entity<ShipmentStatusLog>(entity =>
            {
                entity.HasOne(sl => sl.Shipment)
                      .WithMany(s => s.StatusLogs)
                      .HasForeignKey(sl => sl.ShipmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

             // Configures relationship between ShipmentTracking and Shipment
            // ShipmentTracking 
            modelBuilder.Entity<ShipmentTracking>(entity =>
            {
                entity.HasOne(st => st.Shipment)
                      .WithMany(s => s.TrackingSteps)
                      .HasForeignKey(st => st.ShipmentId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            //  Seed default admin user 
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "Admin",
                    Email = "admin@aquafreight.com",
                    PasswordHash = "admin123",  // Password: admin123 (plain for demo — in production use BCrypt)
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.Now,
                }
            );

            //  Seed Data 
            SeedData(modelBuilder);
        }

        private static void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ship>().HasData(
                new Ship { Id = 1, ShipCode = "S001", ShipName = "MV Titan", ImoNumber = "9876543", CapacityMT = 52000, ShipType = "Bulk Carrier", FlagCountry = "India", YearBuilt = 2018 },
                new Ship { Id = 2, ShipCode = "S002", ShipName = "MV Neptune", ImoNumber = "9765432", CapacityMT = 38000, ShipType = "Container", FlagCountry = "India", YearBuilt = 2020 },
                new Ship { Id = 3, ShipCode = "S003", ShipName = "MV Horizon", ImoNumber = "9654321", CapacityMT = 65000, ShipType = "Tanker", FlagCountry = "Singapore", YearBuilt = 2016 },
                new Ship { Id = 4, ShipCode = "S004", ShipName = "MV Orion", ImoNumber = "9543210", CapacityMT = 28500, ShipType = "Cargo", FlagCountry = "UK", YearBuilt = 2019 },
                new Ship { Id = 5, ShipCode = "S005", ShipName = "MV Atlas", ImoNumber = "9432109", CapacityMT = 71000, ShipType = "Bulk Carrier", FlagCountry = "India", YearBuilt = 2015 }
            );

            modelBuilder.Entity<Customer>().HasData(
                new Customer { Id = 1, CustomerCode = "C001", CustomerName = "Rajesh Mehta", CompanyName = "Reliance Industries", Email = "r.mehta@reliance.com", Phone = "+91 98765 43210", City = "Mumbai" },
                new Customer { Id = 2, CustomerCode = "C002", CustomerName = "Anita Sharma", CompanyName = "Tata Steel", Email = "anita.s@tatasteel.com", Phone = "+91 87654 32109", City = "Jamshedpur" },
                new Customer { Id = 3, CustomerCode = "C003", CustomerName = "Vikram Nair", CompanyName = "ONGC", Email = "v.nair@ongc.in", Phone = "+91 76543 21098", City = "Dehradun" },
                new Customer { Id = 4, CustomerCode = "C004", CustomerName = "Priya Desai", CompanyName = "Adani Ports", Email = "p.desai@adani.com", Phone = "+91 65432 10987", City = "Ahmedabad" },
                new Customer { Id = 5, CustomerCode = "C005", CustomerName = "Suresh Kumar", CompanyName = "L&T Shipping", Email = "s.kumar@lt.com", Phone = "+91 54321 09876", City = "Chennai" }
            );

            modelBuilder.Entity<Port>().HasData(
                new Port { Id = 1, PortCode = "INBOM", PortName = "Mumbai Port", Country = "India", City = "Mumbai" },
                new Port { Id = 2, PortCode = "INJNP", PortName = "JNPT Port", Country = "India", City = "Navi Mumbai" },
                new Port { Id = 3, PortCode = "INMAA", PortName = "Chennai Port", Country = "India", City = "Chennai" },
                new Port { Id = 4, PortCode = "INKND", PortName = "Kandla Port", Country = "India", City = "Kandla" },
                new Port { Id = 5, PortCode = "AEJEA", PortName = "Jebel Ali Dubai", Country = "UAE", City = "Dubai" },
                new Port { Id = 6, PortCode = "SGSIN", PortName = "Singapore PSA", Country = "Singapore", City = "Singapore" },
                new Port { Id = 7, PortCode = "NLRTM", PortName = "Port of Rotterdam", Country = "Netherlands", City = "Rotterdam" }
            );
        }
    }
}