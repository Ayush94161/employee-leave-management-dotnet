# Employee Leave Management System

A resume-ready ASP.NET Core 8 Web API for managing employees and leave requests. It demonstrates REST APIs, EF Core, SQLite persistence, JWT authentication, validation, automated tests, Docker, and Jenkins CI/CD.

## Features

- Employee creation and listing
- Submit, approve, or reject leave requests
- Automatic leave-balance validation and deduction
- JWT-based admin authorization
- Swagger/OpenAPI documentation
- SQLite database with persistent Docker volume
- Health-check endpoint and xUnit integration tests
- Multi-stage Docker build and Jenkins deployment pipeline

## Tech stack

ASP.NET Core 8, C#, Entity Framework Core, SQLite, JWT, Swagger, xUnit, Docker, Docker Compose, Jenkins

## Run with Docker

```bash
cp .env.example .env
docker compose up --build -d
```

Open Swagger at `http://localhost:8080/swagger` and health check at `http://localhost:8080/health`.

Default development login:

```json
POST /api/auth/login
{
  "username": "admin",
  "password": "Admin@123"
}
```

Copy the returned token and enter `Bearer <token>` in Swagger's Authorize dialog. Change all default credentials before a real deployment.

## API endpoints

| Method | Endpoint | Access | Purpose |
|---|---|---|---|
| POST | `/api/auth/login` | Public | Get admin JWT |
| GET | `/api/employees` | Public | List employees |
| GET | `/api/employees/{id}` | Public | Get employee |
| POST | `/api/employees` | Admin | Add employee |
| GET | `/api/leave-requests?status=Pending` | Public | List/filter requests |
| POST | `/api/leave-requests` | Public | Submit request |
| PATCH | `/api/leave-requests/{id}/status` | Admin | Approve/reject request |
| GET | `/health` | Public | Container health check |

## Local development

Requires .NET 8 SDK.

```bash
dotnet restore
dotnet test
dotnet run --project src/LeaveManagement.Api
```

## Jenkins CI/CD

The `Jenkinsfile` performs checkout, Docker-based build and tests, runtime image creation, Docker Compose deployment, and an HTTP health check. Jenkins agent requirements: Git, Docker, Docker Compose, and permission to use the Docker daemon.

Create a Jenkins Pipeline job, select **Pipeline script from SCM**, choose Git, add this repository URL, and keep the script path as `Jenkinsfile`.

## Resume description

**Employee Leave Management System | ASP.NET Core, EF Core, Docker, Jenkins**

- Developed a RESTful employee leave-management API using ASP.NET Core 8, EF Core, SQLite, JWT authentication, validation, and Swagger.
- Containerized the application with a multi-stage Docker build and persistent volume; added xUnit API tests and a health-check endpoint.
- Implemented a Jenkins declarative CI/CD pipeline to build, test, package, deploy with Docker Compose, and validate application health automatically.

## Author

Ayush Dhiman
