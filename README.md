## Library Management System

### Overview
The Library Management System is a comprehensive solution for managing library operations, including book inventory, user management, and loan tracking. This project is built using .NET Core and follows a multi-project approach, separating concerns into distinct layers.

### Features
* Book management (CRUD)
* User management (CRUD)
* Loan processing (CRUD + functionality for setting the book as burrowed/returned)
* Automated notifications for due dates (Notification only mocked)
* RESTful API for integration with other systems
* Robust error handling and logging

### Technology Stack
* .NET 8.0
* Entity Framework Core
* PostgreSQL
* AutoMapper
* Serilog for logging
* xUnit for unit and integration testing
* Hangfire for background job processing
* Swagger for API documentation
* Docker for containment - to deploy on render.com

### Structure

The project is organized into multiple sub-projects:

* **LibraryManagementSystem.API**: The API project defines the REST endpoints for interacting with the library system.
* **LibraryManagementSystem.Core**: This project contains the core business logic and domain models.
* **LibraryManagementSystem.Infrastructure**: This project provides the data access layer, potentially using a database or other storage mechanisms.
* **LibraryManagementSystem.IntegrationTests**: This project includes integration tests to validate the API's functionality and its interaction with the infrastructure.
* **LibraryManagementSystem.UnitTests**: This project contains unit tests for the core business logic and domain models.

### Getting Started

* **Check the app online:**
    * The hosted application can be find here: https://librarymanagementsystem-t1kd.onrender.com
(sometimes it takes a minute to load - hosting service needs to start after sleeping :P) 
    * Both the application and the PostgreSQL DB are hosted on https://render.com.

* **Prerequisites:**
    * .NET 8 SDK
    * A suitable database (depending on the implementation)
* **Installation:**
    1. Clone the repository.
    2. Run `dotnet restore` to download dependencies.
    3. Build the solution using `dotnet build`.
* **Running the Application Locally:**
    1. Make sure you have configured your database settings in the `appsettings.json` file. 
    (Don't change the connection string to use the hosted database).
    2. Start the API project using `dotnet run` (from the `LibraryManagementSystem.API` directory).
* **Running Tests:**
    1. Execute the integration tests using `dotnet test` (from the `LibraryManagementSystem.IntegrationTests` directory).
    2. Run the unit tests using `dotnet test` (from the `LibraryManagementSystem.UnitTests` directory).
