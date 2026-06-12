# Task Management System

## Problem

Internal teams require a lightweight task management system to manage and track work items efficiently.

The system should allow users to:

* Create tasks
* Assign tasks
* Track task status
* Track task priority
* Update tasks
* Soft delete tasks
* View task summaries

The solution should provide a RESTful API for task management operations and a React-based user interface for end users.

## Solution

Develop a full-stack Task Management System using .NET 8, React, SQL Server, and Entity Framework Core.

The application will provide:

* RESTful APIs for task management operations
* React-based Single Page Application (SPA)
* SQL Server database using Entity Framework Core Code First migrations approach
* Input validation and centralized error handling
* Soft delete functionality for auditability
* Task filtering and reporting capabilities
* Database seed data for immediate application usage

In addition to the core requirements, the solution will include authentication, unit testing, and containerization support.

## Features

### Task Management

The system will support the following operations:

* List all tasks
* Retrieve task details by Id
* Create a new task
* Update an existing task
* Soft delete a task

### Task Filtering

Users can filter tasks using:

* Status
* Priority

### Task Status Management

Supported statuses:

* To Do
* In Progress
* Done

### Task Priority Management

Supported priorities:

* Low
* Medium
* High
* Critical

### Task Summary

The system will provide a summary endpoint that returns task counts grouped by:

* Status
* Priority

This endpoint will be implemented using a raw SQL query as required.

### Authentication

The system will provide:

* User Login
* JWT Token Authentication
* Protected API Endpoints
* Protected React Routes
* Logout Functionality

### Seed Data

The database will be pre-populated using Entity Framework Core seed data.

Initial seed data will contain:

* Default Admin User
* At least 10 Sample Tasks

## Functional Requirements

### Backend Requirements

The API must support:

* GET /api/tasks
* GET /api/tasks/{id}
* POST /api/tasks
* PUT /api/tasks/{id}
* DELETE /api/tasks/{id}
* GET /api/tasks/summary

Authentication endpoint:

* POST /api/auth/login

The API should:

* Support filtering by Status
* Support filtering by Priority
* Return consistent API responses
* Validate incoming requests
* Support soft deletion
* Secure endpoints using JWT Authentication

### Frontend Requirements

The React application must provide:

* Login page
* Task dashboard
* Task listing page
* Task creation form
* Task update functionality
* Status update dropdown
* Priority indicators
* Logout functionality
* Responsive and user-friendly interface

## Non-Functional Requirements

### Validation

The system should enforce:

* Required title validation
* Valid status values
* Valid priority values
* Request model validation

### Error Handling

The application should provide:

* Global exception handling
* Consistent error response structure
* Meaningful validation messages

### Logging

The application should provide:

* Request logging
* Error logging

### Performance

The application should:

* Use asynchronous programming
* Optimize database access
* Use efficient LINQ queries

### Security

The solution should implement:

* JWT Authentication
* Protected API Endpoints
* Secure credential handling

### Maintainability

The application should:

* Follow SOLID principles
* Use dependency injection
* Follow Clean Architecture principles
* Maintain separation of concerns

## Architecture

### Architecture Style

Clean Architecture

### Presentation Layer

Responsibilities:

* API Controllers
* Middleware
* Authentication
* Swagger Configuration

### Application Layer

Responsibilities:

* DTOs
* Business Services
* Validators
* Interfaces
* Application Logic

### Domain Layer

Responsibilities:

* Entities
* Enums
* Domain Rules
* Core Business Models

### Infrastructure Layer

Responsibilities:

* Entity Framework Core
* Database Access
* Repository Implementations

## Database

### Database Engine

* SQL Server LocalDB

### Data Access Strategy

* Entity Framework Core
* Code First Approach
* Migrations
* Seed Data

### Tables

#### Users

Purpose:

* Authentication
* Authorization
* Task Ownership

#### Tasks

Purpose:

* Task Management
* Status Tracking
* Priority Tracking
* Assignment Tracking
* Soft Delete Support

## Bonus Features

### Authentication

* JWT Authentication
* Protected API Endpoints

### Unit Testing

Unit tests will be added for service layer and business logic components.

### Docker Support

Containerization support for:

* API
* SQL Server
* React Application

Using:

* Docker
* Docker Compose

### Shared .NET Standard Library

A shared library will be created to demonstrate cross-project code reuse.

Shared components may include:

* API Response Models
* Shared Enums
* Common Constants
* Utility Classes

## Technology Stack

### Backend

* .NET 8
* ASP.NET Core Web API
* Entity Framework Core
* SQL Server

### Frontend

* React
* Axios
* React Hooks
* Context API

### Testing

* xUnit
* Moq
* FluentAssertions
