# 🏥 PureClinic.Backend

**PureClinic.Backend** is a high-performance, scalable, and secure **ASP.NET Core Web API** designed to streamline clinic management operations. It offers robust features for managing **patients, doctors, appointments, bookings, medical services**, and more.

With a **clean architecture** and **modern software design principles**, this system ensures flexibility, maintainability, and efficiency for medical institutions.

## 🔧 **Key Features (In Progress)**

✔️ **Patient Management** – Seamless patient registration, record management, and history tracking.  
✔️ **Doctor Management** – Maintain detailed doctor profiles, schedules, and specialties.  
✔️ **Appointment/ Booking Management** – Efficient appointment scheduling, tracking, and notifications.  
✔️ **Medical Services** – Manage clinic services, treatments, and billing.  
✔️ **Real-time Notifications (SignalR)** – Instant updates for doctors, patients, and staff.  
✔️ **Memory Cache & Redis Cache** – Optimized performance with caching strategies.  
✔️ **Background Job Processing (Hangfire)** – Asynchronous task handling for better system responsiveness.  
✔️ **File Storage with Cloudinary** – Securely store and manage medical records and images.  
✔️ **SMTP Integration** – Automated email notifications for appointments and updates.  
✔️ **Logging & Monitoring (Serilog)** – Robust logging mechanisms for debugging and analytics.  
✔️ **Authentication & Authorization** – Secure access control with JWT & Binary Permissions.  
✔️ **Repository & Unit of Work Design Pattern** – Maintainable and testable data access structure.  
✔️ **Unit test (Xunit, Mock)** - for Code Reliability

## 🏛 **Project Structure** 
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
└── README.md                   # Project documentation 
```
![Clean architecture](https://github.com/hieptrinh2002/PureClinic.Backend/blob/master/CleanArchitecture.png)


---

## 🛠 **Clean Code & Best Practices in PureClinic.Backend**
....

## 🚀 Getting Started

1. Clone the repository:
   ```bash
   git clone https://github.com/hieptrinh2002/PureClinic.Backend.git
   cd PureClinic.Backend
