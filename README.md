# CryptoPulse ReadMe

## Project Overview

CryptoPulse is a cryptocurrency tracking web application built using .NET MVC, Entity Framework, and C# with a MSSQL database. It provides users with up-to-date information on the top cryptocurrencies, their associated markets, and details about various exchanges.

## Features

1. **Coins Page**
   - Displays information about the top cryptocurrencies.
   - Utilizes the [Coinlore Cryptocurrency Data API](https://www.coinlore.com/cryptocurrency-data-api) for real-time data.

2. **Markets Page**
   - Provides detailed information about the markets associated with each coin.
   - Implements one-to-many relationships between coins and markets.

3. **Exchanges Page**
   - Offers insights into different cryptocurrency exchanges.
   - Establishes one-to-many relationships between exchanges and markets.

4. **Watchlist (For Logged-in Users)**
   - Allows registered users to add and remove coins from their watchlist for easy tracking.

## Database Schema

- **Entities**
  - Coin
  - Market
  - Exchange
  - User

- **Relationships**
  - One-to-Many: Coin to Markets
  - One-to-Many: Exchange to Markets
  - One-to-Many: Coin to Users (for Watchlist)

## How to Set Up and Run the Project
## Database Configuration
## Apply Migrations
- Run the following commands in the terminal:
- dotnet ef migrations add cryptodb
- dotnet ef database update
. **Clone the Repository**
   ```bash
   git clone https://github.com/GaganDeepAlusuri/CryptoPulse.git

### Open `appsettings.json`

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  },
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=CryptoPulseDb;Trusted_Connection=True;MultipleActiveResultSets=true"
  }
}
