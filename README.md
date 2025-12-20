# TaskManager API

A full-stack web application for managing tasks with authentication and CRUD functionality.

## Project Overview

TaskManager API is a full-stack application that allows users to create accounts, log in, and manage their tasks. The application is built with React (frontend) and ASP.NET Core (backend) with PostgreSQL as the database.

### Features

- User authentication (registration and login)
- Create, read, update, and delete tasks (CRUD)
- Mark tasks as completed
- Set task priorities
- Deadline management for tasks

## Tech Stack

### Backend
- **Framework:** ASP.NET Core 9.0
- **Database:** PostgreSQL (via Npgsql.EntityFrameworkCore.PostgreSQL 9.0.4)
- **ORM:** Entity Framework Core 9.0.6
- **Authentication:** ASP.NET Core Identity 9.0.6
- **API Documentation:** Scalar.AspNetCore 2.4.22

### Frontend
- **Framework:** React
- **Language:** JavaScript

### Testing
- **Test Framework:** xUnit 2.9.2
- **Mocking:** Moq 4.20.72
- **Test SDK:** Microsoft.NET.Test.Sdk 17.11.1
- **Code Coverage:** coverlet.collector 6.0.2
- **Test Runner:** xunit.runner.visualstudio 2.8.2

## System Requirements

- .NET SDK 9.0 or later
- Node.js and npm (for frontend)
- PostgreSQL database

## Installation

### 1. Clone the repository

```bash
git clone <repository-url>
cd TaskManagerApi
```

### 2. Install backend dependencies

```bash
dotnet restore
```

### 3. Configure the database

Update your database connection string in `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=taskmanagerdb;Username=youruser;Password=yourpassword"
  }
}
```

### 4. Run database migrations

```bash
dotnet ef database update
```

### 5. Install frontend dependencies

```bash
cd ClientApp
npm install
cd ..
```

## Running the Application

### Start the application

From the project root directory:

```bash
dotnet run
```

This starts both the backend server and the frontend application. Your browser will automatically open and display the login page.

### First-time use

1. Navigate to the registration page
2. Create a new user account
3. Log in with your credentials
4. Start creating and managing your tasks

## Running Tests

The application contains unit tests for TaskService and ITaskRepository with the following tested methods:
- `CreateTaskAsync` - Create new task
- `GetAllTasksAsync` - Retrieve all tasks for a user
- `GetTaskAsync` - Retrieve a specific task
- `DeleteTaskAsync` - Delete a task
- `CompleteTaskAsync` - Mark task as completed

### Run the tests

1. Navigate to the test project:

```bash
cd TaskManagerApiTests
```

2. Build the test project:

```bash
dotnet build
```

3. Run all tests:

```bash
dotnet test
```

### Test Results

The tests use Moq to mock dependencies and ensure that the service layer functions correctly without affecting the database.

## Project Structure

```
TaskManagerApi/
├── Controllers/          # API controllers
├── Models/              # Data models and DTOs
├── Services/            # Business logic
├── Repositories/        # Data access layer
├── Data/                # DbContext and configuration
├── Frontend\client/           # React, JavaScript
└── appsettings.json     # Configuration file

TaskManagerApiTests/
├── TaskServiceTest.cs   # Unit tests for TaskService
└── TaskManagerApiTests.csproj
```

## API Endpoints

### Authentication
Note: The login and register endpoints are taken care by the identity core built in methods.
- `POST /user/register` - Register new user
- `POST /user/login` - Log in

### Tasks
- `GET /api/task/` - Retrieve all tasks
- `GET /api/task/{taskId}` - Retrieve specific task
- `POST /api/task/create` - Create new task
- `PUT /api/task/{taskId}` - Update task
- `DELETE /api/task/{taskId}` - Delete task
- `PATCH /api/task/complete` - Mark task as completed

## Testing Rationale

I have chosen to focus the tests on TaskService and its interaction with ITaskRepository because:

1. **Core Business Logic:** TaskService contains the majority of the application's business logic for task management.

2. **Critical Operations:** CRUD operations are fundamental to the application's functionality and must be reliable.

3. **User Experience:** These methods directly affect the user's ability to manage their tasks.

4. **Isolated Testing:** By mocking ITaskRepository, we can test the service layer in isolation from the database, resulting in faster and more reliable tests.

5. **Complete Functionality:** The CompleteTaskAsync method is an important feature that needs to be verified separately.

The tests ensure that:
- New tasks are created correctly with the right user ID and properties
- All user tasks can be retrieved
- Individual tasks can be retrieved by ID
- Tasks can be safely deleted
- Tasks can be marked as completed

For more detailed information about the testing approach and methodology, see the [Test Documentation](TestDocumentation.md).

## Package Dependencies

### TaskManagerApi (Backend)
```
Microsoft.AspNetCore.Identity.EntityFrameworkCore 9.0.6
Microsoft.AspNetCore.OpenApi 9.0.4
Microsoft.EntityFrameworkCore.Design 9.0.6
Npgsql.EntityFrameworkCore.PostgreSQL 9.0.4
Scalar.AspNetCore 2.4.22
```

### TaskManagerApiTests
```
coverlet.collector 6.0.2
Microsoft.NET.Test.Sdk 17.11.1
Moq 4.20.72
xunit 2.9.2
xunit.runner.visualstudio 2.8.2
```


## Troubleshooting

### Common Issues

**Issue:** Database cannot be accessed
- **Solution:** Check that PostgreSQL is running and that the connection string is correct

**Issue:** Frontend does not load
- **Solution:** Check that npm packages are installed in the ClientApp folder

**Issue:** Tests fail
- **Solution:** Run `dotnet clean`, `dotnet restore`, `dotnet build` before `dotnet test` to ensure all dependencies are updated

