# Financial Data Tracker (.NET 8 Web API)

This project is a technical assessment for the Rasyonet Software Engineering Internship. It fetches real-time stock data from the Alpha Vantage API, stores it in a local SQLite database, and provides analytical views.

## 🚀 Key Features & Architectural Decisions
- **Service Pattern & DI:** Business logic is decoupled from Controllers to ensure high maintainability.
- **Data Abstraction (DTOs):** Database entities are protected using `StockDto` to satisfy data encapsulation requirements.
- **Analytical View:** Includes a specialized endpoint to list stocks ordered by price (descending).
- **Global Error Handling:** Implemented clean `ProblemDetails` responses instead of raw system exceptions.

## 🛠 Tech Stack
- **Framework:** .NET 8.0 (Web API)
- **Database:** SQLite (Chosen for portability)
- **ORM:** Entity Framework Core
- **Bonus:** Docker Support included.

## ⚙️ Setup & Run

### 1. API Key Configuration (CRITICAL)
For security reasons, the API key has been removed from `appsettings.json`. 
1. Get a free key from [Alpha Vantage](https://www.alphavantage.co/).
2. Replace `"YOUR_API_KEY_HERE"` in `appsettings.json` with your actual key.

### 2. Running with .NET CLI
```bash
dotnet build
dotnet run
