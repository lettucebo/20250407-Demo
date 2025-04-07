# Subscription Tracker

## Overview
Subscription Tracker is a web application designed to help users manage their subscriptions efficiently. It provides features such as tracking subscription costs, upcoming payments, and category breakdowns. The application is built using ASP.NET Core 8.0 and follows best practices for clean architecture and maintainability.

## Features
- **Dashboard**: View total monthly costs, upcoming payments, and category breakdowns.
- **Category Management**: Create, edit, and delete subscription categories.
- **Subscription Management**: Create, edit, and delete subscriptions with detailed information.
- **Responsive Design**: Optimized for both desktop and mobile devices.
- **Data Visualization**: Visualize category breakdowns using charts.

## Prerequisites
- **Operating System**: Windows 11
- **.NET SDK**: .NET 8.0
- **Database**: In-memory database (for development and testing)

## Getting Started

### 1. Clone the Repository
```bash
git clone <repository-url>
cd SubscriptionTracker
```

### 2. Restore Dependencies
```bash
cd src
 dotnet restore
```

### 3. Build the Solution
```bash
dotnet build
```

### 4. Run the Application
```bash
dotnet run --project SubscriptionTracker.Web
```

The application will be available at `http://localhost:5000` or `https://localhost:5001`.

## Project Structure
```
src/
  SubscriptionTracker.sln
  SubscriptionTracker.Web/
    Controllers/
    Data/
    Models/
    Repositories/
    Services/
    Views/
    wwwroot/
  SubscriptionTracker.Tests/
    Services/
```

## Testing
### Unit Tests
Run the following command to execute unit tests:
```bash
dotnet test
```

## Documentation
- **Implementation Plans**: Located in `/docs/implementation`
- **Architecture Decision Records (ADRs)**: Located in `/docs/adr`
- **Changelog**: See `CHANGELOG.md`

## Contributing
1. Fork the repository.
2. Create a new branch for your feature or bugfix.
3. Commit your changes with detailed messages.
4. Submit a pull request.

## License
This project is licensed under the MIT License. See `LICENSE` for details.