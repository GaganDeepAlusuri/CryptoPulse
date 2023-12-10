# CryptoPulse ReadMe

## Project Overview

CryptoPulse is a cryptocurrency tracking web application built using .NET MVC, Entity Framework, and C# with a MSSQL database. It provides users with up-to-date information on the top cryptocurrencies, their associated markets, and details about various exchanges.

## Features

1. **Home Page**
   - Overview of the application's features and functionalities.
   - ![Home Page](https://imgur.com/3wdsiWS)

2. **Coins Page**
   - Displays information about the top cryptocurrencies.
   - Utilizes the [Coinlore Cryptocurrency Data API](https://www.coinlore.com/cryptocurrency-data-api) for real-time data.
   - ![Coins Page](https://imgur.com/taWrpxV)

3. **Markets Page**
   - Provides detailed information about the markets associated with each coin.
   - ![Markets Page](https://imgur.com/91pUFvd)

4. **Exchanges Page**
   - Offers insights into different cryptocurrency exchanges.
   - ![Exchanges Page](https://imgur.com/3ckhBwb)

5. **Watchlist (For Logged-in Users)**
   - Allows registered users to add and remove coins from their watchlist for easy tracking.
   - ![Watchlist Feature](https://imgur.com/p83K6Os)

6. **Register Page**
   - User registration interface.
   - ![Register Page](https://imgur.com/OKgf8t8)

7. **Login Page**
   - User login interface.
   - ![Login Page](https://imgur.com/GvYzToh)

## CI/CD Implementation
- CI/CD has been implemented using GitHub workflows to streamline development and deployment processes.

## Azure Cloud Resource Group

- CryptoPulse utilizes Azure Cloud services for enhanced performance and scalability.
- Detailed information about the Azure resource group used for deployment and management of the application's cloud resources.
- ![Azure Resource Group](https://imgur.com/F4IoJR1) (https://imgur.com/TQloUAb)
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
