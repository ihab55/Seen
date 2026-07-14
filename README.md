# Seen - Sports Training Management System

A comprehensive ASP.NET Core Web API for managing sports training programs, teams, athletes, and sensor data collection with role-based access control.

## 🏗️ Architecture Overview

The project follows a **4-layer architecture** with clear separation of concerns:

```
┌─────────────────────────────────────────────────────────┐
│              SeenPL (Presentation Layer)                │
│         ASP.NET Core Web API Controllers                │
└─────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────┐
│              SeenBLL (Business Logic Layer)            │
│                   Services & Business Logic             │
└─────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────┐
│               SeenCL (Common Layer)                     │
│      DTOs, Domain Entities, Interfaces, Contracts      │
└─────────────────────────────────────────────────────────┘
                            ↓
┌─────────────────────────────────────────────────────────┐
│              SeenDAL (Data Access Layer)                │
│              Repositories & Database Operations          │
└─────────────────────────────────────────────────────────┘
```

### Layer Responsibilities

- **SeenPL**: RESTful API endpoints, request/response handling, authentication middleware
- **SeenBLL**: Business logic, validation, service orchestration, JWT authentication
- **SeenCL**: Shared data models, DTOs, domain entities, interface definitions
- **SeenDAL**: Database operations, SQL queries, connection management

## 🔧 Technology Stack

- **.NET 8.0**: Core framework
- **ASP.NET Core**: Web API framework
- **SQL Server**: Database backend
- **BCrypt.Net**: Password hashing
- **JWT (System.IdentityModel.Tokens.Jwt)**: Token-based authentication
- **Swagger/OpenAPI**: API documentation
- **Dependency Injection**: Built-in DI container

## 🔐 Authentication & Authorization

### JWT Authentication Implementation

The application implements a comprehensive JWT-based authentication system with the following features:

#### AuthService
Located in `SeenBLL.Services`, the `AuthService` class handles JWT authentication for Users and Admins with the following responsibilities:

- **Credential Validation**: Uses BCrypt password verification for secure authentication
- **JWT Access Tokens**: Issues signed JWT tokens with role claims (User, Coach, Admin)
- **Refresh Tokens**: Issues and persists opaque refresh tokens for extended sessions
- **Token Rotation**: Rotates refresh tokens on use (old token revoked, new one issued)
- **Token Revocation**: Supports token revocation for logout functionality

#### Security Features

- **BCrypt Password Hashing**: Secure password storage and verification
- **Role-Based Authorization**: Granular access control with User, Coach, and Admin roles
- **Token Refresh Flow**: Secure token rotation to maintain session validity
- **Token Revocation**: Immediate invalidation of tokens on logout

#### User Roles

- **User**: Regular athletes/players with access to personal data and assigned programs
- **Coach**: Can create training programs, manage teams, and monitor athlete performance
- **Admin**: System administrator with full access to user management and system configuration

## 📦 Core Features

### User Management
- User registration and profile management
- Coach account creation and approval workflow
- Profile completion tracking
- User status management (active/deleted)

### Team Management
- Team creation and management
- Team member roster management
- Coach-to-player assignments
- Team overview and statistics

### Training Programs
- Create and manage training programs
- Program comments and feedback
- Program assignment to athletes
- Progress tracking

### Sensor Data Collection
- Device registration and management
- Real-time sensor data ingestion
- Performance metrics tracking (heart rate, speed, etc.)
- Historical data analysis

### Notifications & Alerts
- System notifications for users
- Alert generation based on sensor data
- Notification delivery and tracking

### Subscriptions
- Subscription plan management
- User subscription tracking
- Plan activation and expiration

## 🏛️ Architecture Patterns

### Repository Pattern
Implements the Repository Pattern for data access abstraction, providing:
- Separation of business logic from data access logic
- Improved testability through mockable data access
- Centralized data access operations
- Consistent data access interface across entities

### Dependency Injection
Utilizes Dependency Injection for:
- Loose coupling between components
- Improved maintainability and testability
- Configurable service lifetimes (Singleton, Scoped)
- Clear dependency management

### Service Layer Pattern
Business logic encapsulated in service classes:
- `UserService`: User operations and profile management
- `AuthService`: Authentication and token management
- `TeamService`: Team creation and management
- `TrainingProgramService`: Program lifecycle management
- `SensorDataService`: Sensor data processing
- And 10+ additional domain-specific services

## 🗄️ Database Schema

The application uses SQL Server with the following key entities:

- **Users**: Athlete and coach accounts
- **Admins**: System administrators
- **Teams**: Team definitions and settings
- **TeamMembers**: Team membership relationships
- **TrainingPrograms**: Workout and training plans
- **ProgramComments**: Feedback on programs
- **Devices**: Registered sensor devices
- **SensorData**: Collected sensor measurements
- **Alerts**: System-generated alerts
- **Notifications**: User notifications
- **Subscriptions**: User subscription plans
- **CoachApprovals**: Coach registration approvals
- **RefreshTokens**: JWT refresh token storage

## ⚙️ Configuration

### appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server;Database=SeenDB;Trusted_Connection=true;"
  },
  "JwtSettings": {
    "SecretKey": "your-secret-key",
    "Issuer": "SeenAPI",
    "Audience": "SeenClient",
    "AccessTokenExpiryMinutes": "60",
    "RefreshTokenExpiryDays": "7"
  }
}
```

## 🚀 Setup Instructions

### Prerequisites
- .NET 8.0 SDK
- SQL Server
- Visual Studio 2022 or VS Code

### Database Setup
1. Create a SQL Server database
2. Run the provided SQL scripts from `MetaData.sql`
3. Configure connection string in `appsettings.json`

### Running the Application
1. Clone the repository
2. Navigate to the solution directory
3. Restore dependencies:
   ```bash
   dotnet restore
   ```
4. Build the solution:
   ```bash
   dotnet build
   ```
5. Run the API:
   ```bash
   dotnet run --project SeenPL/SeenPL.csproj
   ```
6. Access Swagger UI at `https://localhost:5001/swagger`

## 📡 API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/admin/login` - Admin login
- `POST /api/auth/refresh` - Refresh access token
- `POST /api/auth/revoke` - Revoke refresh token

### Users
- `GET /api/users` - Get all users
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user
- `PUT /api/users/{id}` - Update user profile
- `DELETE /api/users/{id}` - Delete user

### Teams
- `GET /api/teams` - Get all teams
- `POST /api/teams` - Create new team
- `GET /api/teams/{id}/members` - Get team members
- `POST /api/teams/{id}/members` - Add member to team

### Training Programs
- `GET /api/programs` - Get all training programs
- `POST /api/programs` - Create training program
- `GET /api/programs/{id}` - Get program details
- `POST /api/programs/{id}/comments` - Add comment to program

### Sensor Data
- `POST /api/sensordata` - Submit sensor data
- `GET /api/sensordata/user/{userId}` - Get user sensor data
- `GET /api/devices` - Get registered devices

*And many more endpoints for complete system management*

## 🧪 Testing

The project structure supports unit testing through:
- Interface-based repository pattern for mocking
- Service layer separation for business logic testing
- DTO pattern for request/response validation

## 📝 Project Structure

```
Seen/
├── SeenPL/                 # Presentation Layer
│   ├── Controllers/        # API Controllers
│   ├── Program.cs          # Application entry point
│   └── appsettings.json   # Configuration
├── SeenBLL/               # Business Logic Layer
│   └── Services/          # Business logic services
├── SeenCL/                # Common Layer
│   ├── DTOs/             # Data Transfer Objects
│   ├── Domain/           # Domain entities
│   ├── Interfaces/       # Service interfaces
│   └── Repositories/     # Repository interfaces
└── SeenDAL/              # Data Access Layer
    ├── Infrastructure/    # Database helpers
    └── Repositories/     # Repository implementations
```

## 🔒 Security Considerations

- All passwords are hashed using BCrypt
- JWT tokens are signed with HMAC-SHA256
- Refresh tokens are stored securely in the database
- Token rotation prevents replay attacks
- Role-based authorization ensures proper access control
- SQL injection prevention through parameterized queries

## 📄 License

This project is part of a graduation project for sports training management.

## 👥 Contributors

- Development team for graduation project 
