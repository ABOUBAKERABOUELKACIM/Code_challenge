# Doctor Office Management System


# Frontend
cd frontend/

npm install

npm run dev

# Backend

A comprehensive healthcare management platform built with Clean Architecture, CQRS, and modern .NET practices.

## Architecture

- **Clean Architecture** with Domain, Application, Infrastructure, and API layers
- **CQRS pattern** using MediatR for command/query separation
- **Domain Events** for business logic decoupling
- **JWT Authentication** with role-based authorization
- **FluentValidation** for input validation
- **Entity Framework Core** with Code-First migrations
- **Docker Compose** for containerized deployment

## Technologies

- .NET 8.0
- Entity Framework Core
- MediatR (CQRS)
- JWT Bearer Authentication
- FluentValidation
- Docker
- xUnit (Unit Testing)
- Swagger/OpenAPI

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- Docker & Docker Compose
- Visual Studio 2022 (optional)

### Local Development
```bash
git clone [repository-url]
cd Backendapiservice
dotnet restore
dotnet run
```

### Docker Deployment
```bash
docker-compose up --build
```

## API Documentation

Access Swagger UI at: `https://localhost:5000/swagger`

### Authentication
1. POST `/api/auth/login` with credentials
2. Use returned JWT token in Authorization header: `Bearer {token}`

### Default Users
- **Admin**: admin@doctoroffice.com / Admin123!

## Project Structure
```
├── Domain/                 # Business entities and domain logic
├── Application/           # Use cases, DTOs, handlers
├── Infrastructure/        # Data access, external services  
├── Controllers/          # API endpoints
└── Tests/               # Unit tests
```

## API Endpoints

### Authentication
- POST `/api/auth/login` - User login

### Offices
- GET `/api/office` - List all offices
- POST `/api/office` - Create office (Admin only)
- PUT `/api/office/{id}` - Update office (Admin only)
- DELETE `/api/office/{id}` - Delete office (Admin only)

### Doctors
- GET `/api/doctor` - List all doctors
- POST `/api/doctor` - Create doctor
- PUT `/api/doctor/{id}` - Update doctor
- DELETE `/api/doctor/{id}` - Delete doctor

### Patients
- GET `/api/patient` - List all patients
- POST `/api/patient` - Create patient
- PUT `/api/patient/{id}` - Update patient
- DELETE `/api/patient/{id}` - Delete patient

### Appointments
- GET `/api/appointment` - List all appointments
- POST `/api/appointment` - Create appointment
- PUT `/api/appointment/{id}` - Update appointment
- PUT `/api/appointment/{id}/status` - Update appointment status
- DELETE `/api/appointment/{id}` - Delete appointment

### Assistants
- GET `/api/assistant` - List all assistants
- POST `/api/assistant` - Create assistant
- PUT `/api/assistant/{id}` - Update assistant
- DELETE `/api/assistant/{id}` - Delete assistant

## Testing

```bash
dotnet test
```

## Features Demonstrated

- Clean Architecture principles
- CQRS with MediatR
- Domain-driven design patterns
- JWT authentication & authorization
- Input validation with FluentValidation
- Domain events for decoupled business logic
- Unit testing with mocking
- Docker containerization
- RESTful API design
- Swagger documentation
```

