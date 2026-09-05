# 🏆 Tournament Table

A full-stack **Tournament Management Web Application** built with **ASP.NET Core MVC**, designed to manage tournaments, participants, matches, brackets, and tournament workflows through a structured web-based platform.

The project was developed as a relatively large-scale academic/software-engineering project, with a focus on backend architecture, database design, authentication, tournament business logic, and interactive frontend components.

---

## 📌 Overview

**Tournament Table** is a web application for creating and managing competitive tournaments.

The system provides a complete workflow starting from tournament creation and participant management, through match generation and results, to displaying the tournament bracket and final standings.

The project combines:

* ASP.NET Core MVC
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* Razor Views
* HTML / CSS / JavaScript
* REST-style endpoints
* Client-side API communication
* Email integration
* Database migrations

The goal was not simply to build a CRUD application, but to implement a real-world domain with non-trivial business rules and state transitions.

---

# 🎯 Project Goals

The main goals of the project were to practice and implement:

* Real-world MVC architecture
* Database-driven web applications
* Entity Framework Core
* Authentication and authorization
* Role-based application logic
* Tournament business rules
* Dynamic bracket generation
* Match management
* Client/server communication
* Email-based workflows
* Responsive and interactive frontend development
* Maintainable backend architecture

---

# ✨ Main Features

## 🏆 Tournament Management

Users can create and manage tournaments through the application.

Tournament-related functionality includes:

* Creating tournaments
* Editing tournament information
* Managing tournament participants
* Managing tournament status
* Starting tournament stages
* Tracking matches
* Displaying tournament information

---

## 👥 Participant Management

The system provides functionality for managing participants associated with tournaments.

Depending on the tournament workflow, participants can be:

* Added to a tournament
* Removed
* Assigned to matches
* Advanced to subsequent rounds
* Eliminated from the tournament

Participant information is persisted through the application's database layer.

---

# 🧩 Tournament Bracket

One of the main technical parts of the project is the **tournament bracket system**.

The application dynamically generates and displays tournament rounds and matches.

A simplified tournament flow looks like:

```text
Tournament
     │
     ▼
Participants
     │
     ▼
Match Generation
     │
     ▼
┌──────────────┐
│   Round 1    │
└──────┬───────┘
       │
       ▼
┌──────────────┐
│ Quarterfinal │
└──────┬───────┘
       │
       ▼
┌──────────────┐
│  Semifinal   │
└──────┬───────┘
       │
       ▼
┌──────────────┐
│    Final     │
└──────┬───────┘
       │
       ▼
     Winner
```

The bracket is generated based on tournament participants and match results.

When a match is completed, the winner can progress to the appropriate next stage.

---

# ⚽ Match Management

Matches represent one of the core entities of the application.

The system handles concepts such as:

* Match participants
* Match rounds
* Match results
* Winners
* Match status
* Tournament progression

The result of one match can affect the participants of another match in the next round.

This creates a dependency chain between matches and requires the application to maintain tournament state consistently.

---

# 🔐 Authentication & Authorization

The application uses **ASP.NET Core Identity** for user management.

Authentication-related functionality includes:

* User registration
* User login
* User logout
* User account management
* Password management
* Role-based authorization

The project also includes email-related functionality for account workflows.

A simplified authentication flow:

```text
Registration
     │
     ▼
Create Account
     │
     ▼
Email Verification
     │
     ▼
Confirmed Account
     │
     ▼
Login
     │
     ▼
Authenticated Session
```

---

# 📧 Email Integration

The application includes email functionality as part of its authentication workflow.

The project was designed to support email delivery for operations such as:

* Account confirmation
* Authentication-related notifications
* User verification

The email layer can be configured independently from the main application logic.

---

# 🗄️ Database

The application uses a relational database architecture powered by **SQL Server** and **Entity Framework Core**.

Entity Framework Core is responsible for:

* Object-relational mapping
* Database queries
* Entity relationships
* Data persistence
* Database migrations

The application follows a code-first approach where the database schema can be evolved through EF Core migrations.

---

# 🔄 Entity Framework Core Migrations

Database changes are tracked through migrations.

Typical migration commands:

```bash
dotnet ef migrations add MigrationName
```

Apply migrations:

```bash
dotnet ef database update
```

This allows the database schema to evolve alongside the application's domain models.

---

# 🏗️ Architecture

The project follows the **ASP.NET Core MVC** architectural pattern.

At a high level:

```text
┌──────────────────────────────────────┐
│              Browser                 │
│                                      │
│       HTML / CSS / JavaScript        │
└──────────────────┬───────────────────┘
                   │
                   ▼
┌──────────────────────────────────────┐
│             Controllers              │
│                                      │
│   Handle HTTP requests and routing    │
└──────────────────┬───────────────────┘
                   │
                   ▼
┌──────────────────────────────────────┐
│                Models                │
│                                      │
│     Domain & application models      │
└──────────────────┬───────────────────┘
                   │
                   ▼
┌──────────────────────────────────────┐
│          Entity Framework Core       │
│                                      │
│       ORM / Data Access Layer        │
└──────────────────┬───────────────────┘
                   │
                   ▼
┌──────────────────────────────────────┐
│              SQL Server              │
└──────────────────────────────────────┘
```

The MVC pattern separates request handling, application/domain data, and presentation.

---

# 🌐 MVC + API Communication

Although the application is primarily built around ASP.NET Core MVC, parts of the system also use API-style communication.

Frontend JavaScript can communicate with backend endpoints through HTTP requests.

For example:

```text
Browser
   │
   │ HTTP Request
   ▼
ASP.NET Core Endpoint
   │
   ▼
Business Logic
   │
   ▼
Entity Framework Core
   │
   ▼
SQL Server
```

This allows interactive parts of the application to update data without requiring a full page reload.

---

# 🎨 Frontend

The frontend is implemented using standard web technologies:

* HTML
* CSS
* JavaScript
* Bootstrap
* Razor Views

The interface includes dynamic components for tournament management and bracket visualization.

JavaScript is used for client-side interactions and communication with backend endpoints.

---

# 📊 Tournament State

A tournament is not simply a collection of independent CRUD records.

The system has to maintain relationships between:

```text
Tournament
     │
     ├── Participants
     │
     ├── Rounds
     │      │
     │      └── Matches
     │             │
     │             ├── Participant A
     │             ├── Participant B
     │             └── Winner
     │
     └── Final Result
```

When a match result changes, the state of the tournament can change as well.

This makes tournament progression one of the more challenging business-logic components of the application.

---

# 🧠 Business Logic

A major focus of the project is implementing domain-specific logic instead of treating the application as a simple CRUD system.

Examples include:

* Validating tournament state
* Managing participants
* Creating matches
* Determining winners
* Progressing participants between rounds
* Preventing invalid tournament operations
* Maintaining relationships between rounds
* Rendering the correct bracket state

The tournament workflow can therefore be represented as:

```text
Create Tournament
       │
       ▼
Add Participants
       │
       ▼
Start Tournament
       │
       ▼
Generate Matches
       │
       ▼
Play Matches
       │
       ▼
Record Results
       │
       ▼
Advance Winners
       │
       ▼
Generate Next Round
       │
       ▼
Continue Until Final
       │
       ▼
Declare Winner
```

---

# 🧪 Validation

The application validates user input before performing important operations.

Validation is especially important for operations such as:

* Creating tournaments
* Registering participants
* Updating match results
* Starting tournaments
* Modifying tournament state

The objective is to prevent invalid data from reaching the database or corrupting tournament state.

---

# 🛡️ Security Considerations

The project uses ASP.NET Core's authentication infrastructure to protect authenticated functionality.

Security-related considerations include:

* Authentication
* Authorization
* Password handling through ASP.NET Core Identity
* Email confirmation
* Anti-forgery protection
* Server-side validation
* Database access through EF Core

Sensitive configuration values such as:

```text
Connection Strings
Email Credentials
Authentication Secrets
```

should be stored outside source control in production environments.

---

# 📁 Project Structure

A simplified structure of the application:

```text
Tournament-Table/
│
├── Controllers/
│   ├── AccountController.cs
│   ├── TournamentController.cs
│   ├── MatchController.cs
│   └── ...
│
├── Models/
│   ├── Tournament.cs
│   ├── Participant.cs
│   ├── Match.cs
│   ├── Round.cs
│   └── ...
│
├── Data/
│   ├── ApplicationDbContext.cs
│   └── ...
│
├── Views/
│   ├── Account/
│   ├── Tournament/
│   ├── Match/
│   └── Shared/
│
├── wwwroot/
│   ├── css/
│   ├── js/
│   └── ...
│
├── Migrations/
│
├── Program.cs
├── appsettings.json
└── ...
```

---

# ⚙️ Technology Stack

| Technology                | Purpose                           |
| ------------------------- | --------------------------------- |
| **C#**                    | Main programming language         |
| **ASP.NET Core MVC**      | Web application framework         |
| **Entity Framework Core** | ORM / data access                 |
| **SQL Server**            | Relational database               |
| **ASP.NET Core Identity** | Authentication & authorization    |
| **Razor**                 | Server-side UI rendering          |
| **HTML5**                 | Application structure             |
| **CSS3**                  | Styling                           |
| **JavaScript**            | Client-side interactions          |
| **Bootstrap**             | UI components / responsive layout |
| **Swagger / HTTP APIs**   | API development/testing           |
| **SMTP / Email Provider** | Email delivery                    |

---

# 🚀 Getting Started

## Prerequisites

Make sure the following tools are installed:

* .NET SDK
* SQL Server
* Git
* Visual Studio / JetBrains Rider / VS Code

---

## Clone the Repository

```bash
git clone https://github.com/Sobhankhedry/tournament-table.git

cd tournament-table
```

---

## Configure the Database

Configure the SQL Server connection string in:

```text
appsettings.json
```

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_CONNECTION_STRING"
  }
}
```

Do not commit production credentials to the repository.

---

## Apply Database Migrations

```bash
dotnet ef database update
```

---

## Restore Dependencies

```bash
dotnet restore
```

---

## Build

```bash
dotnet build
```

---

## Run

```bash
dotnet run
```

The application will print the local HTTP/HTTPS addresses in the terminal.

Open the displayed address in your browser.

---

# 🧭 Typical User Flow

A typical tournament workflow looks like:

```text
              ┌──────────────┐
              │     User     │
              └──────┬───────┘
                     │
                     ▼
              ┌──────────────┐
              │    Login     │
              └──────┬───────┘
                     │
                     ▼
          ┌──────────────────────┐
          │ Create Tournament    │
          └──────────┬───────────┘
                     │
                     ▼
          ┌──────────────────────┐
          │ Add Participants     │
          └──────────┬───────────┘
                     │
                     ▼
          ┌──────────────────────┐
          │ Start Tournament     │
          └──────────┬───────────┘
                     │
                     ▼
          ┌──────────────────────┐
          │ Generate Bracket     │
          └──────────┬───────────┘
                     │
                     ▼
          ┌──────────────────────┐
          │ Record Match Results  │
          └──────────┬───────────┘
                     │
                     ▼
          ┌──────────────────────┐
          │ Advance Participants │
          └──────────┬───────────┘
                     │
                     ▼
          ┌──────────────────────┐
          │      Final Match     │
          └──────────┬───────────┘
                     │
                     ▼
               🏆 Winner
```

---

# 💡 Technical Challenges

One of the main challenges of the project was managing the relationship between the application's data model and tournament state.

Unlike a conventional CRUD application, tournament operations can have cascading effects.

For example:

```text
Match Result
     │
     ▼
Winner Determination
     │
     ▼
Next Match
     │
     ▼
Bracket Update
     │
     ▼
Tournament State
```

The application therefore needs to maintain consistency between the database, backend business logic, and frontend representation.

---

# 🔮 Future Improvements

Possible improvements for a production-ready version include:

* [ ] Complete automated test suite
* [ ] Unit tests for tournament business rules
* [ ] Integration tests
* [ ] More granular service layer
* [ ] Repository abstraction where beneficial
* [ ] DTO-based API contracts
* [ ] FluentValidation
* [ ] Global exception handling
* [ ] Structured logging
* [ ] JWT authentication for API clients
* [ ] Dockerization
* [ ] CI/CD pipeline
* [ ] Production deployment
* [ ] Real-time match updates with SignalR
* [ ] Tournament notifications
* [ ] Advanced tournament formats
* [ ] Improved bracket visualization
* [ ] Performance optimization
* [ ] Caching with Redis

---

# 📚 What This Project Demonstrates

This project demonstrates practical experience with:

### Backend Development

* ASP.NET Core
* MVC architecture
* Entity Framework Core
* SQL Server
* Database migrations
* Authentication
* Authorization
* REST-style endpoints
* Business logic

### Software Architecture

* Separation of concerns
* MVC pattern
* Domain modeling
* Persistence layer
* Client/server communication
* State management

### Frontend Development

* Razor Views
* HTML
* CSS
* JavaScript
* Bootstrap
* Dynamic UI components
* AJAX/API communication

### Domain Modeling

* Tournament entities
* Participants
* Matches
* Rounds
* Tournament progression
* Bracket generation

---

# 🏆 Why This Project Is Different

The main challenge of **Tournament Table** was not creating forms and storing records in a database.

The interesting part was modeling and implementing a system where:

> **Every match can affect the state of the entire tournament.**

This required combining:

```text
Web Development
      +
Database Design
      +
Authentication
      +
Business Logic
      +
Tournament Algorithms
      +
Frontend Interaction
```

into one integrated application.

---

# 📌 Project Status

🚧 **Academic / Personal Project**

The project was developed as a substantial software-engineering project and provides a foundation for a complete tournament management platform.

---

# 👨‍💻 Author

**Sobhan Khedry**

GitHub:

https://github.com/Sobhankhedry
