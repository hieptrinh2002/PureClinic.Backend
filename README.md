
# PureClinic.Backend

**PureClinic.Backend** is an API system built with .NET designed to manage clinic operations. It provides features for managing patients, doctors, appointments, and medical services...

---

## 🔧 **Key Features (In progress)**

- **Patient Management**:
- **Doctor Management**:
- **Appointment Management**:
- **Medical Services**:
- **Realtime notification**:
- ...............

# ASP.NET Core Web API: Secure, Scalable, and Elegant (Clean Architecture)
## Project Structure
```
├── src
│   ├── Core                    # Contains the core business logic and domain models, view models, etc.
│   ├── Infrastructure          # Contains infrastructure concerns such as data access, external services, etc.
│   └── API                      # Contains the API layer, including controllers, extensions, etc.
├── tests
│   ├── Core.Tests              # Contains unit tests for the core layer
│   ├── Infrastructure.Tests    # Contains unit tests for the infrastructure layer
│   └── API.Tests                # Contains unit tests for the API layer
└── README.md                   # Project documentation (you are here!)
```


## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/hieptrinh2002/PureClinic.Backend.git
   cd PureClinic.Backend

## ✨ Features

- **Clean architecture with SOLID principles.**
- **CRUD operations** for managing patients, doctors, appointments, and services.
- **Repository and Unit of Work patterns** for clean data access.
- **Entity Framework Core** as the ORM.
- **ASP.NET Core API** with JWT-based authentication.
- **API versioning** for backward compatibility.
- **Dependency injection** and modular design.
- **Unit testing support.**

---

## 📂 Key Components

- **Models**: Domain entities for patients, doctors, and appointments.
- **Repositories**: Data access abstraction using Entity Framework Core.
- **Services**: Business logic and orchestration.
- **Controllers**: RESTful API endpoints for CRUD operations.

