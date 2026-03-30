# 🚢 ShipmentManagement — Shipment Tracking & Booking System

A full-featured web-based Shipment Management System built with
ASP.NET Core MVC, Entity Framework Core, and SQL Server.

---

## 🛠️ Tech Stack

- ASP.NET Core MVC (.NET 6+)
- Entity Framework Core
- SQL Server / LocalDB
- xUnit + Moq + FluentAssertions (Unit Testing)
- Chart.js (Dashboard Charts)
- Bootstrap Icons

---

## ✅ Features Implemented

- Shipment Booking with Auto-generated ID (SHP-YYYY-XXXX)
- Shipment List with Search & Filter (by Status, Type)
- Shipment Details & Timeline Tracking
- Manual Status Update with History Log
- Auto-mark shipment as Delayed if arrival date exceeded
- Ship Master — Full CRUD
- Customer Master — Full CRUD (Email locked on edit)
- REST API with DTO responses
- Login & Session Authentication
- Dashboard with Charts (Line, Pie, Bar)

---

## 🌐 REST API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/shipments | All shipments |
| GET | /api/shipments/{id} | Shipment by ID |
| GET | /api/shipments/status/{status} | Filter by status |
| GET | /api/shipments/code/{code} | By shipment code |
| GET | /api/shipments/stats | Dashboard stats |

---

## ⚙️ Setup Instructions

### 1. Clone the repository
git clone https://github.com/YOUR_USERNAME/ShipmentManagement.git

### 2. Update Connection String
Open `ShipmentManagement/appsettings.json`
Change the `DefaultConnection` to your SQL Server instance.

### 3. Run Migrations
Open Package Manager Console in Visual Studio:
Add-Migration InitialCreate
Update-Database

### 4. Run the Application
Press F5 in Visual Studio
OR
cd ShipmentManagement
dotnet run

### 5. Default Login Credentials
Email:    admin@aquafreight.com
Password: admin123

---

## 🧪 Running Unit Tests

cd ShipmentManagement.Tests
dotnet test --verbosity normal

Total Tests: 47
All Passing: ✅ 47/47

### Test Coverage Includes:
- ShipmentService — Create, AutoDelay, StatusUpdate, GetAll
- ShipService — CRUD, Search, Filter, Delete validation
- CustomerService — CRUD, Search, Email lock on edit
- AuthService — Login, Logout, Session validation

---

## 📁 Project Structure

ShipmentManagement/
├── Controllers/         MVC + API Controllers
├── Models/              Entity models + Enums
├── Views/               Razor views (.cshtml)
├── Services/            Business logic layer
├── Repositories/        Data access layer
├── DTOs/                API response objects
├── ViewModels/          View-specific models
├── Filters/             Auth filter
├── Data/                AppDbContext + Migrations
└── wwwroot/             CSS, JS, static files

ShipmentManagement.Tests/
├── Tests/Services/      Unit tests (xUnit + Moq)
└── Mocks/               Mock data for testing

---

## 🏗️ Architecture

Controller → ViewModel → Service → Repository → DbContext → SQL Server

- MVC separation of concerns
- Repository pattern for data access
- Service layer for all business logic
- Async/await throughout
- DTOs for API responses
