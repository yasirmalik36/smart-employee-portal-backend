# 🏢 Smart Employee Portal — Backend API

> Enterprise-grade HR & attendance management system built with **.NET Core 8**, **Clean Architecture**, and **AI-driven analytics**. Features real-time notifications, biometric face recognition attendance, and role-based access control.

[![.NET](https://img.shields.io/badge/.NET_Core_8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=for-the-badge)](LICENSE)

---

## ✨ Features

- 👤 **User Management** — Role-based access control (Admin, Manager, Employee) with JWT authentication
- 🕐 **Attendance Tracking** — Clock-in/clock-out with biometric face recognition integration
- 🏖️ **Leave Management** — Leave requests, approvals, balance tracking with email notifications
- ✅ **Task Management** — Assign, track, and report on tasks with priority levels
- 📊 **AI Analytics Dashboard** — Attendance patterns, productivity metrics, and anomaly detection
- 🔔 **Real-time Notifications** — SignalR-powered instant alerts for approvals and updates
- 📈 **Reports** — Attendance, payroll summary, and performance reports with export

---

## 🏗️ Architecture

```
SmartEmpAPI/
├── API/                    # Controllers, Middleware, Filters
│   ├── Controllers/        # RESTful endpoints
│   └── Middleware/         # Auth, error handling, logging
├── Application/            # Business logic (CQRS)
│   ├── Commands/           # Write operations
│   ├── Queries/            # Read operations
│   └── DTOs/               # Data transfer objects
├── Domain/                 # Core entities & interfaces
│   ├── Entities/           # User, Employee, Attendance, Leave, Task
│   └── Interfaces/         # Repository contracts
└── Infrastructure/         # Data access & external services
    ├── Persistence/        # EF Core, SQL Server
    ├── Repositories/       # Data access implementations
    └── Services/           # Email, notifications, AI
```

**Design Patterns Used:**
- ✅ Clean Architecture (Domain → Application → Infrastructure → API)
- ✅ CQRS with MediatR — commands and queries fully separated
- ✅ Repository Pattern — abstracted data access layer
- ✅ JWT Authentication with refresh tokens
- ✅ Generic Repository with Unit of Work

---

## 🔧 Tech Stack

| Layer | Technology |
|-------|-----------|
| Framework | .NET Core 8 / ASP.NET Core Web API |
| Language | C# 12 |
| Database | SQL Server 2022 |
| ORM | Entity Framework Core 8 |
| Auth | JWT Bearer Tokens |
| Real-time | SignalR |
| Docs | Swagger / OpenAPI |
| Testing | xUnit, Moq |

---

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### 1. Clone the Repository

```bash
git clone https://github.com/yasirmalik36/smart-employee-portal-backend.git
cd smart-employee-portal-backend/SmartEmpAPI
```

### 2. Configure Database

Update `appsettings.json` with your SQL Server connection string:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER;Database=SmartEmployeePortal;Trusted_Connection=True;TrustServerCertificate=True"
  },
  "JwtSettings": {
    "SecretKey": "your-256-bit-secret-key-here",
    "Issuer": "SmartEmployeePortal",
    "Audience": "SmartEmployeePortalUsers",
    "ExpiryMinutes": 60
  }
}
```

### 3. Run Database Migrations

```bash
dotnet ef database update
```

### 4. Run the API

```bash
dotnet run
```

API will be available at `https://localhost:7001` · Swagger UI at `https://localhost:7001/swagger`

---

## 📡 API Endpoints

### Authentication
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/auth/login` | Login and get JWT token |
| `POST` | `/api/auth/refresh` | Refresh access token |
| `POST` | `/api/auth/logout` | Invalidate refresh token |

### Employees
| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/employees` | Get all employees (paginated) |
| `GET` | `/api/employees/{id}` | Get employee by ID |
| `POST` | `/api/employees` | Create new employee |
| `PUT` | `/api/employees/{id}` | Update employee |
| `DELETE` | `/api/employees/{id}` | Soft delete employee |

### Attendance
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/attendance/clock-in` | Record clock-in (with optional face recognition) |
| `POST` | `/api/attendance/clock-out` | Record clock-out |
| `GET` | `/api/attendance/report` | Get attendance report by date range |

### Leave Management
| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/leaves/request` | Submit leave request |
| `PUT` | `/api/leaves/{id}/approve` | Approve/reject leave (Manager) |
| `GET` | `/api/leaves/balance` | Get leave balance |

---

## 🔗 Related Repositories

| Repo | Description |
|------|-------------|
| [smart-employee-portal-frontend](https://github.com/yasirmalik36/smart-employee-portal-frontend) | Angular 17 frontend |
| [smart-employee-portal-face-recognition](https://github.com/yasirmalik36/smart-employee-portal-face-recognition) | Python face recognition service |

---

## 🤝 Author

**Yasir Mehmood** — Senior Full-Stack & AI Engineer

[![Portfolio](https://img.shields.io/badge/Portfolio-Visit-1B3A6B?style=flat-square)](https://yasir-portfolio-omega.vercel.app)
[![LinkedIn](https://img.shields.io/badge/LinkedIn-Connect-0077B5?style=flat-square&logo=linkedin)](https://linkedin.com/in/yasir-mehmood-53549a1b3)
[![Email](https://img.shields.io/badge/Email-Contact-D14836?style=flat-square&logo=gmail)](mailto:yash36114@gmail.com)

---

## 📄 License

This project is licensed under the MIT License — see the [LICENSE](LICENSE) file for details.
