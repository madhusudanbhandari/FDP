#  Food Delivery Platform

A full-stack food delivery platform built with **ASP.NET Core Web API, React, PostgreSQL, Redis, and Docker**.

The project was developed to practice and demonstrate real-world backend concepts such as **REST APIs, JWT authentication, role-based authorization, Entity Framework Core, repository/service architecture, Redis caching, background services, SignalR, Docker, health checks, and production-oriented application design**.

---

##  Tech Stack

### Backend

* **ASP.NET Core Web API**
* **.NET 10**
* **Entity Framework Core**
* **PostgreSQL**
* **Redis**
* **JWT Authentication**
* **BCrypt**
* **FluentValidation**
* **AutoMapper**
* **Serilog**
* **SignalR**
* **Swagger / OpenAPI**
* **Health Checks**
* **Background Services**
* **Docker**

### Frontend

* **React**
* **Vite**
* **Axios**
* **React Router**
* **Docker**
* **Nginx**

### Infrastructure

* Docker
* Docker Compose
* PostgreSQL container
* Redis container
* ASP.NET Core API container
* React/Nginx container

---

#  Features

## Authentication & Authorization

* User registration
* User login
* JWT-based authentication
* Role-based authorization
* Password hashing with BCrypt
* Protected API endpoints

### Roles

* Customer
* Restaurant Owner
* Delivery Person
* Admin

---

##  Restaurant Management

* Create restaurants
* Update restaurants
* Browse restaurants
* Search restaurants
* Filter by open/closed status
* Sort by rating
* Pagination
* Restaurant owner authorization

Example:

```http
GET /api/Restaurant?page=1&pageSize=10&search=pizza&isOpen=true&sortBy=rating&sortOrder=desc
```

---

##  Menu Management

* Create menus
* Add menu items
* Update menu items
* Delete menu items
* Check item availability
* Restaurant owner authorization

---

##  Cart

* One cart per user
* Add items
* Increase/decrease quantity
* Remove items
* View current cart
* Automatically calculate cart totals

---

##  Orders

* Create orders from cart
* Order status management
* Order history
* Restaurant owner order management
* Order cancellation
* Order/payment relationship

Example order lifecycle:

```text
Pending
   ↓
Confirmed
   ↓
Preparing
   ↓
Ready
   ↓
OutForDelivery
   ↓
Delivered
```

---

## Payments

* Payment creation
* Payment status tracking
* Mock payment provider
* Payment linked with orders
* Automatic cancellation of unpaid expired orders

---

##  Delivery

* Delivery creation
* Assign delivery person
* Track delivery status
* Pickup tracking
* Delivery completion tracking

Example:

```text
Assigned
   ↓
PickedUp
   ↓
Delivered
```

---

##  Reviews

* Customers can review orders
* Restaurant reviews
* Rating system
* Review management

---

##  Notifications

* User notifications
* Notification persistence
* Real-time notifications using **SignalR**

SignalR hub:

```text
/notificationHub
```

---

#  Redis Caching

Redis is used to cache frequently accessed restaurant data.

The project follows the **Cache-Aside Pattern**:

```text
Client
  ↓
API
  ↓
Check Redis
  ↓
 ┌───────────────┐
 │ Cache exists? │
 └───────────────┘
     ↓       ↓
    Yes      No
     ↓       ↓
 Return    PostgreSQL
             ↓
          Store in Redis
             ↓
           Return
```

Restaurant cache keys contain query parameters so different searches/pagination results can be cached independently.

Example:

```text
fdp:restaurants:p1:ps10:search=pizza:open=true:sort=rating:order=desc:
```

Redis cache also uses **TTL** and cache invalidation when restaurant data changes.

---

#  Backend Architecture

The backend follows a layered architecture:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
Entity Framework Core
    ↓
PostgreSQL
```

Cross-cutting infrastructure includes:

```text
JWT Authentication
Global Exception Middleware
Validation
Logging
Redis
SignalR
Health Checks
Background Services
```

### Example

```text
RestaurantController
        ↓
IRestaurantService
        ↓
RestaurantService
        ↓
IRestaurantRepository
        ↓
RestaurantRepository
        ↓
AppDbContext
        ↓
PostgreSQL
```

This separation keeps business logic out of controllers and makes the application easier to maintain and test.

---

#  Database

The application uses **PostgreSQL** with Entity Framework Core.

Major entities include:

```text
Users
Restaurants
Menus
MenuItems
Carts
CartItems
Orders
Payments
Deliveries
Reviews
Notifications
```

Entity relationships are configured using EF Core.

Database migrations are automatically applied when the application starts:

```csharp
db.Database.Migrate();
```

---

# Background Service

The project contains a hosted background service:

```text
OrderCleanupService
```

It periodically checks for orders that:

* Are still pending
* Have a pending payment
* Have exceeded the payment timeout

Expired orders are automatically cancelled.

The service uses:

```csharp
BackgroundService
IServiceScopeFactory
CancellationToken
```

to safely work with scoped dependencies.

---

#  Logging

The application uses **Serilog** for structured logging.

Logs are written to:

```text
Console
Logs/log.txt
```

Daily rolling logs are configured.

Example:

```text
Logs/
├── log-2026-09-19.txt
├── log-2026-09-20.txt
└── ...
```

Logging is used for:

* Application events
* Errors
* Background jobs
* Order processing
* Debugging

---

#  Health Checks

The application exposes:

```http
GET /health
```

Health checks verify important infrastructure such as:

```text
ASP.NET Core API
      ↓
PostgreSQL
      ↓
Redis
```

A healthy response confirms that the required services are available.

---

#  Docker

The complete application is containerized.

Docker services:

```text
┌──────────────────────────────┐
│        Docker Compose        │
│                              │
│  React/Nginx                 │
│       ↓                      │
│  FDP API                     │
│    ↙     ↘                   │
│ PostgreSQL  Redis             │
│                              │
└──────────────────────────────┘
```

### Start the application

Clone the repository:

```bash
git clone <your-repository-url>
cd FoodDeliveryPlatform
```

Create a `.env` file:

```env
POSTGRES_PASSWORD=your_password

DB_CONNECTION=Host=postgres;Port=5432;Database=FoodDeliveryPlatform;Username=postgres;Password=your_password

JWT_KEY=your_jwt_secret
```

Then run:

```bash
docker compose up --build
```

To stop the application:

```bash
docker compose down
```

---

#  Environment Configuration

Sensitive configuration is supplied through environment variables.

Example:

```yaml
environment:
  ConnectionStrings__DefaultConnection: ${DB_CONNECTION}
  Redis__ConnectionString: redis:6379
  Jwt__Key: ${JWT_KEY}
```

The `.env` file should **never be committed to Git**.

Make sure it is included in `.gitignore`:

```gitignore
.env
```

---

#  API

The backend API is exposed through:

```text
http://localhost:5107
```

Health check:

```text
http://localhost:5107/health
```

Swagger can be enabled for API development/testing.

---

#  Security

The project implements:

* JWT authentication
* Role-based authorization
* Password hashing
* Protected API endpoints
* User-specific resource access
* Restaurant-owner authorization
* Environment-based secrets
* CORS configuration

Protected requests use:

```http
Authorization: Bearer <JWT>
```

---

#  Testing

Testing is planned/being expanded around:

* Service logic
* Repository behavior
* API endpoints
* Authentication/authorization
* Integration with PostgreSQL

---

# Project Structure

```text
FoodDeliveryPlatform/
│
├── Controller/
│
├── Services/
│
├── Repository/
│
├── Interface/
│
├── Models/
│
├── Dtos/
│
├── Data/
│
├── Middleware/
│
├── Validators/
│
├── Profiles/
│
├── Hubs/
│
├── Exceptions/
│
├── Migrations/
│
├── Dockerfile
├── docker-compose.yml
├── .env
├── .gitignore
│
└── Frontend/
    ├── src/
    ├── public/
    ├── Dockerfile
    └── nginx.conf
```

---

# Concepts Practiced

This project was built as a practical learning project for modern .NET development.

Key concepts include:

* C#
* OOP
* SOLID principles
* Dependency Injection
* REST APIs
* HTTP methods/status codes
* Routing
* Middleware
* Controllers
* DTOs
* Repository Pattern
* Service Layer
* Entity Framework Core
* LINQ
* PostgreSQL
* Database migrations
* JWT
* Authentication & Authorization
* Role-based authorization
* Redis
* Cache-Aside Pattern
* TTL
* Cache invalidation
* SignalR
* Background Services
* Structured logging
* Health checks
* Docker
* Docker Compose
* React
* Axios
* Nginx
* Environment configuration

---

# Future Improvements

Planned improvements include:

* Automated unit and integration tests
* GitHub Actions CI/CD
* Production deployment
* HTTPS
* Rate limiting
* Monitoring
* Database backup strategy
* Improved observability
* Cloud deployment
* Better frontend UI/UX

---

#  Project Goal

The goal of this project is to build a realistic full-stack food delivery application while learning how modern **ASP.NET Core applications are designed, secured, cached, containerized, tested, and prepared for deployment**.

---


Built as a practical full-stack .NET learning and portfolio project.
