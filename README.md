# E-commerce API

This project is the backend API for an E-commerce platform, built using ASP.NET Core and following Clean Architecture principles. It provides functionalities for user management, authentication, product catalog, shopping cart, checkout, and administrative tasks.

## Features Implemented

* **Authentication & Authorization:**
    * User Registration (`POST /users/register`)
    * User Login (`POST /users/login`) returning JWT
    * User Logout (`POST /users/logout`) (Server-side acknowledgement, client handles token deletion)
    * JWT Bearer token validation for protected routes
    * Role-based authorization (e.g., "AdminOnly" policy)
* **User Profile Management (for logged-in user):**
    * Get Profile (`GET /users/me`)
    * Update Profile (Name, Telephone) (`PUT /users/me`)
    * Change Password (requires current password) (`PUT /users/me/password`)
    * Add Address (`POST /users/me/addresses`)
    * Add Address via ViaCEP lookup (`POST /users/me/addresses/by-cep`)
    * List Addresses (`GET /users/me/addresses`)
    * Update Address (`PUT /users/me/addresses/{id}`)
    * Delete Address (`DELETE /users/me/addresses/{id}`)
* **Shopping Cart (for logged-in user):**
    * Add Item to Cart (`POST /cart/items`) - handles quantity update if item exists, stock validation
    * Get Cart Contents (`GET /cart`) - includes item details and total amount
    * Update Item Quantity (`PUT /cart/items/{itemId}`) - includes stock validation
    * Clear Entire Cart (`DELETE /cart`)
* **Orders (Checkout & History for logged-in user):**
    * Create Order (Checkout) (`POST /orders`) - requires address, validates cart/stock, decrements stock, clears cart within a transaction
    * Get Order History (`GET /orders`)
* **Public Catalog:**
    * List Products (`GET /products`) - with pagination, filtering (category, price, search term), and sorting
    * List Categories (`GET /categories`)
* **Admin Management (requires 'admin' role):**
    * List All Users (`GET /admin/users`)
    * Create Category (`POST /admin/categories`)
    * Update Category (`PUT /admin/categories/{id}`) - regenerates slug, checks for conflicts
    * Delete Category (`DELETE /admin/categories/{id}`) - prevents deletion if products are associated
    * Create Product (`POST /admin/products`)
    * Update Product (`PUT /admin/products/{id}`)
    * Delete Product (`DELETE /admin/products/{id}`)
    * List All Orders (`GET /admin/orders`) - includes user details
    * Update Order Status (`PUT /admin/orders/{id}/status`)

## Technology Stack

* **Framework:** ASP.NET Core (.NET 8 recommended, adapt packages if using .NET 6/7)
* **Language:** C#
* **Architecture:** Clean Architecture
* **Database:** PostgreSQL
* **ORM:** Entity Framework Core (Code-First)
* **Authentication:** JWT Bearer Tokens
* **Validation:** FluentValidation
* **API Documentation:** Swagger (Swashbuckle)
* **External API:** ViaCEP (for address lookup by Zip Code)

## Project Structure (Clean Architecture)

The solution is organized into the following layers, adhering to the Clean Architecture dependency rule (dependencies point inwards):

* **`Ecommerce.Domain`**: Contains core business entities, enums, interfaces for repositories, and domain logic. Has no external dependencies.
* **`Ecommerce.Application`**: Contains application logic, use cases (business workflows), DTO validation (FluentValidation), security helpers (JWT generation, password hashing), and interfaces for external services. Depends only on `Domain`.
* **`Ecommerce.Infrastructure`**: Implements data access (EF Core DbContext, Repositories using PostgreSQL/Npgsql), and potentially external services (like email). Depends on `Application` and `Domain`.
* **`Ecommerce.API`**: The ASP.NET Core Web API project. Contains controllers, middleware, dependency injection configuration (`Program.cs`), and `appsettings.json`. Depends on `Application` and `Infrastructure`.
* **`Ecommerce.Communication`**: Contains Data Transfer Objects (DTOs) used for API requests and responses. Shared across layers but primarily used by `API` and `Application`.
* **`Ecommerce.Exceptions`**: Contains custom exception classes used throughout the application.

## Setup & Installation

1.  **Prerequisites:**
    * .NET SDK (Version compatible with the project, e.g., .NET 8 SDK)
    * PostgreSQL Server (running locally or accessible)
    * A PostgreSQL client tool (like DBeaver or pgAdmin) is recommended.

2.  **Clone the Repository:**
    ```bash
    git clone https://github.com/lucasmartinssw/E-commerce-API
    cd E-commerce-API
    ```

3.  **Database Setup:**
    * Using your PostgreSQL client tool, create a **new, empty database** (e.g., `ecommerce_db`).
    * Update the connection string in `src/Ecommerce.API/appsettings.Development.json`. Replace placeholders with your actual server address, database name, user, and password:
        ```json
        {
          "ConnectionStrings": {
            "DefaultConnection": "Server=localhost;Port=5432;Database=ecommerce_db;User Id=your_user;Password=your_password;Trust Server Certificate=true;"
          },
          // ... other settings
        }
        ```
        *(`Trust Server Certificate=true` might be needed for local connections without proper SSL setup).*

4.  **Apply Database Migrations:**
    * Open a terminal/command prompt in the **root directory** of the solution (where the `.sln` file is).
    * Ensure the `dotnet-ef` tool is installed: `dotnet tool install --global dotnet-ef` (or `update`).
    * Run the following command to apply all migrations and create the database schema:
        ```bash
        dotnet ef database update --project src/Ecommerce.Infrastructure --startup-project src/Ecommerce.API
        ```

5.  **Build the Solution:**
    ```bash
    dotnet build
    ```

## Running the Application

1.  **Using .NET CLI:**
    * Navigate to the API project directory: `cd src/Ecommerce.API`
    * Run the application: `dotnet run`

2.  **Using Visual Studio:**
    * Open the `Ecommerce.sln` file in Visual Studio.
    * Set `Ecommerce.API` as the startup project.
    * Press `F5` or click the "Play" button (IIS Express or Kestrel).

The API will start, typically listening on `https://localhost:7XXX` and `http://localhost:5XXX`. The specific ports are shown in the console output.

Navigate to `/swagger` in your browser (e.g., `https://localhost:7XXX/swagger`) to access the Swagger UI for testing endpoints.

## Authentication (JWT)

* The `POST /api/user/register` and `POST /api/user/login` endpoints return a JWT Bearer token upon success.
* To access protected endpoints in Swagger:
    1.  Execute `/login` or `/register` and copy the returned `token` string.
    2.  Click the **"Authorize" 🔒** button (top right).
    3.  In the "Value" field, type `Bearer ` (note the space) and paste your token after it (e.g., `Bearer eyJhbGci...`).
    4.  Click "Authorize" and "Close". The lock icon should now be closed.
    5.  You can now execute protected routes (those without `[AllowAnonymous]`).

## Admin Setup

* The API uses role-based authorization. Routes under `/admin` require the user to have the `admin` role.
* The standard registration (`POST /api/user/register`) assigns the `customer` role by default.
* **To create your first admin:**
    1.  Register a new user via the API.
    2.  Using your database client tool (DBeaver/pgAdmin), connect to your database.
    3.  Open the `users` table.
    4.  Find the user you just registered.
    5.  Manually **edit the `role` column** for that user from `customer` to `admin`.
    6.  Save the changes in the database.
    7.  Log in via the API (`POST /api/user/login`) with this user's credentials. The returned JWT will now contain the `admin` role claim, allowing access to admin routes.
