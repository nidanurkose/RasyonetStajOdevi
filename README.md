# Financial Data Tracker (.NET 6 Web API)

This project is a technical assessment for the Rasyonet Software Engineering Internship. It fetches real-time stock data from the Alpha Vantage API and stores it in a local SQLite database.

## 🚀 Purpose
The application serves as a simple internal tool for financial data tracking, allowing users to fetch specific stock quotes, list historical data, and perform basic analytics (sorting by price).

## 🛠 Tech Stack & Decisions
- **Framework:** .NET 8.0 (Web API)
- **Database:** SQLite (Chosen for portability and ease of setup)
- **API Integration:** Alpha Vantage API (Real-time global quotes)
- **Design Pattern:** **Service Pattern & Dependency Injection.**
  - *Why?* Business logic is decoupled from Controllers to ensure high maintainability and testability.

## ⚙️ Setup & Run
1. Clone the repository.
2. Open the project in VS Code or Visual Studio.
3. Add your Alpha Vantage API Key to `appsettings.json` (or use the default demo key).
4. Run the following command in the terminal:
   ```bash
   dotnet run