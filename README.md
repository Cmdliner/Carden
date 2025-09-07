# Carden API

A comprehensive personal finance management API built with ASP.NET Core that helps users track expenses, manage wallets, and organize their financial data.

## Table of Contents

- [Features](#features)
- [Technology Stack](#technology-stack)
- [Getting Started](#getting-started)
- [API Documentation](#api-documentation)
- [Authentication](#authentication)
- [Database Schema](#database-schema)
- [Configuration](#configuration)
- [Development](#development)
- [Contributing](#contributing)

## Features

- **User Management**
  - User registration and authentication
  - JWT-based authorization with refresh tokens
  - Profile management with image upload support
  - Secure password hashing with BCrypt

- **Expense Tracking**
  - Create, read, update, and delete expense items
  - Categorize expenses with custom categories
  - Priority-based expense organization
  - Track expected prices and purchase dates

- **Wallet Management**
  - Digital wallet creation and management
  - Balance tracking
  - Multi-provider support (Paystack integration)

- **Security Features**
  - JWT authentication with access and refresh tokens
  - Input validation with FluentValidation
  - Global exception handling
  - CORS configuration for frontend integration

## Technology Stack

- **Framework**: ASP.NET Core 9.0
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: JWT Bearer tokens
- **Validation**: FluentValidation
- **File Upload**: Cloudinary integration
- **API Versioning**: Asp.Versioning
- **Password Hashing**: BCrypt.Net

## Getting Started

### Prerequisites

- .NET 9.0 SDK
- PostgreSQL database
- Cloudinary account (for image uploads)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/Cmdliner/Carden.git
   cd Carden
   ```

2. **Install dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the application**
   
   Create `appsettings.Development.json` with the following structure:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Database=carden_db;Username=your_username;Password=your_password"
     },
     "Jwt": {
       "Secret": "your-super-secret-jwt-key-here",
       "Issuer": "https://localhost:7000",
       "Audience": "https://localhost:7000"
     },
     "Cloudinary": {
       "CloudName": "your_cloud_name",
       "ApiKey": "your_api_key",
       "ApiSecret": "your_api_secret"
     }
   }
   ```

4. **Run database migrations**
   ```bash
   cd Carden.Api
   dotnet ef database update
   ```

5. **Start the application**
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:7000` (or the port specified in your launch settings).

## API Documentation

### Base URL
```
https://localhost:7000/api/v1
```

### Authentication Endpoints

#### Register User
```http
POST /api/v1/auth/register
Content-Type: application/json

{
  "email": "user@example.com",
  "full_name": "John Doe",
  "username": "johndoe", // optional
  "password": "SecurePassword123"
}
```

#### Login
```http
POST /api/v1/auth/login
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123"
}
```

**Response:**
```json
{
  "success": true,
  "data": {
    "access_token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
    "refresh_token": "refresh_token_here",
    "user": {
      "id": "uuid",
      "email": "user@example.com",
      "full_name": "John Doe"
    }
  }
}
```

#### Refresh Token
```http
POST /api/v1/auth/refresh
Authorization: Bearer <access_token>
x-refresh: <refresh_token>
```

#### Logout
```http
DELETE /api/v1/auth/logout
Authorization: Bearer <access_token>
```

### User Management Endpoints

#### Get User Profile
```http
GET /api/v1/users
Authorization: Bearer <access_token>
```

#### Update Profile Image
```http
PUT /api/v1/users/profile/image
Authorization: Bearer <access_token>
Content-Type: multipart/form-data

formFile: <image_file>
```

#### Delete User
```http
DELETE /api/v1/users/{user_id}
Authorization: Bearer <access_token>
```

### Expense Item Endpoints

#### Get All Expense Items
```http
GET /api/v1/expense-items
Authorization: Bearer <access_token>
```

#### Get Single Expense Item
```http
GET /api/v1/expense-items/{id}
Authorization: Bearer <access_token>
```

#### Create Expense Item
```http
POST /api/v1/expense-items
Authorization: Bearer <access_token>
Content-Type: application/json

{
  "name": "Laptop",
  "description": "New work laptop",
  "category": "Electronics",
  "expected_price": 1299.99,
  "priority": 1
}
```

#### Update Item Priority
```http
PUT /api/v1/expense-items/{id}/priority
Authorization: Bearer <access_token>
Content-Type: application/json

{
  "new_priority": 2
}
```

#### Delete Expense Item
```http
DELETE /api/v1/expense-items/{id}
Authorization: Bearer <access_token>
```

### Wallet Endpoints

#### Create Wallet
```http
POST /api/v1/wallets/create
Authorization: Bearer <access_token>
```

#### Get Wallet
```http
GET /api/v1/wallets
Authorization: Bearer <access_token>
```

## Authentication

The API uses JWT (JSON Web Tokens) for authentication. After successful login, you'll receive:

- **Access Token**: Short-lived token for API requests (include in Authorization header)
- **Refresh Token**: Long-lived token for obtaining new access tokens

### Token Usage

Include the access token in the Authorization header:
```
Authorization: Bearer <your_access_token>
```

For refresh operations, include both the access token in the Authorization header and the refresh token in the `x-refresh` header.

## Database Schema

### User Entity
- `Id` (Guid, Primary Key)
- `Email` (String, Required, Unique)
- `FullName` (String, Required)
- `Username` (String, Optional)
- `PasswordHash` (String, Required)
- `ProfileImageUrl` (String, Optional)
- `CreatedAt`, `UpdatedAt`, `LastLogin`, `DeletedAt` (DateTime)

### ExpenseItem Entity
- `Id` (Guid, Primary Key)
- `UserId` (Guid, Foreign Key)
- `Name` (String, Required)
- `Category` (String, Optional)
- `Description` (String, Optional)
- `ExpectedPrice` (Decimal, Required)
- `Priority` (UInt, Required)
- `PurchasedAt` (DateTime, Optional)
- `CreatedAt`, `UpdatedAt` (DateTime)

### Wallet Entity
- `Id` (Guid, Primary Key)
- `UserId` (Guid, Foreign Key)
- `Balance` (Decimal, Required)
- `Provider` (Enum: Paystack)
- `CreatedAt`, `UpdatedAt` (DateTime)

### RefreshToken Entity
- `Id` (Guid, Primary Key)
- `UserId` (Guid, Foreign Key)
- `Token` (String, Required)
- `ExpiresAt` (DateTime, Required)

## Configuration

### Required Environment Variables

Create a `appsettings.Development.json` file with:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "PostgreSQL connection string"
  },
  "Jwt": {
    "Secret": "Your JWT secret key (minimum 32 characters)",
    "Issuer": "Your API issuer URL",
    "Audience": "Your API audience URL"
  },
  "Cloudinary": {
    "CloudName": "Your Cloudinary cloud name",
    "ApiKey": "Your Cloudinary API key",
    "ApiSecret": "Your Cloudinary API secret"
  }
}
```

### User Secrets (Development)

For development, you can use user secrets:
```bash
dotnet user-secrets set "Jwt:Secret" "your-secret-key"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"
```

## Development

### Running the Application

1. **Development Mode**:
   ```bash
   dotnet run --environment Development
   ```

2. **Watch Mode** (auto-restart on changes):
   ```bash
   dotnet watch run
   ```

### Database Migrations

1. **Add a new migration**:
   ```bash
   dotnet ef migrations add MigrationName
   ```

2. **Update database**:
   ```bash
   dotnet ef database update
   ```

3. **Remove last migration**:
   ```bash
   dotnet ef migrations remove
   ```

### API Versioning

The API supports versioning through:
- URL segments: `/api/v1/users`
- Headers: `X-Api-Version: 1`

Current API version: **v1**

### Validation

The API uses FluentValidation for input validation. Validation errors are returned in a consistent format:

```json
{
  "success": false,
  "error": {
    "code": "400",
    "message": "Validation failed",
    "details": {
      "Email": ["Email is required"],
      "Password": ["Password must be at least 8 characters"]
    }
  }
}
```

### Error Handling

Global exception handling provides consistent error responses:

```json
{
  "success": false,
  "error": {
    "code": "500",
    "message": "Internal server error"
  },
  "timestamp": "2025-09-08T10:30:00Z"
}
```

### CORS Configuration

CORS is configured to allow requests from `http://localhost:8081` for frontend development. Update the CORS policy in `Program.cs` for production environments.

## Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit your changes (`git commit -m 'Add some amazing feature'`)
4. Push to the branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request

### Code Style

- Follow C# naming conventions
- Use XML documentation for public APIs
- Write unit tests for new features
- Ensure all tests pass before submitting PR

### Testing

Run tests with:
```bash
dotnet test
```

## Security Considerations

- **Password Security**: Passwords are hashed using BCrypt with salt
- **JWT Security**: Use strong secret keys and appropriate expiration times
- **Input Validation**: All inputs are validated using FluentValidation
- **HTTPS**: Always use HTTPS in production
- **Database Security**: Use connection string encryption and least-privilege database accounts

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Support

For questions or support, please open an issue on the GitHub repository or contact the development team.

---

**Happy coding! 🚀**