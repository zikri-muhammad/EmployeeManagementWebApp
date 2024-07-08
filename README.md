# Employee Management Web Application

This project is an Employee Management System built using ASP.NET Core Razor Pages, a PostgreSQL database

## Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/download/dotnet/6.0)

## Project Structure

- `EmployeeManagementWebApp.sln` - Solution file
- `EmployeeManagementWebApp/` - Folder containing the ASP.NET Core project
  - `EmployeeManagementWebApp.csproj` - Project file
  - `Pages/` - Folder containing Razor Pages
  - `Models/` - Folder containing data models
  - `Services/` - Folder containing service classes

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/zikri-muhammad/EmployeeManagementWebApp.git
cd EmployeeManagementWebApp

```

### 2. Configure the Web Application
Open the appsettings.json file in the EmployeeManagementWebApp project and update the PostgreSQL connection string:

```bash
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Port=5432;Database=EmployeeManagement;Username=youruser;Password=yourpassword"
}
```

### 3. Run Application

```bash
cd EmployeeManagementWebApp
dotnet build
dotnet run
```
