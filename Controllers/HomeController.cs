﻿using CryptoPulse.Data;
using CryptoPulse.Infrastructure.CryptoPulseHandler;
using CryptoPulse.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;

namespace CryptoPulse.Controllers
{
    public class HomeController : Controller
    {
        public IdentityDbContext dbContext;
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<IdentityUser> _userManager;
        public const string SessionKeyName = "CoinsData";

        public HomeController(ILogger<HomeController> logger, IdentityDbContext context, UserManager<IdentityUser> userManager)
        {
            _logger = logger;
            dbContext = context;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /****
         * The Coins action calls the GetCoins method that returns a list of Coins.
         * This list of Coins is passed to the Coins View.
        ****/
        public IActionResult Coins()
        {
            try
            {
                // Attempt to retrieve and serialize coin data
                CryptoPulseHandler webHandler = new CryptoPulseHandler();
                List<Coin> coins = webHandler.GetCoins();
                string coinsData = JsonConvert.SerializeObject(coins);
                HttpContext.Session.SetString(SessionKeyName, coinsData);

                // Set the success flag to 1 on success
                ViewBag.dbSucessComp = 1;

                return View(coins);
            }
            catch (Exception ex)
            {
                // Handle the exception here (e.g., log it or set an error flag)
                ViewBag.dbSucessComp = 0;

                // Optionally, you can pass the exception message to the view
                ViewBag.ErrorMessage = ex.Message;

                // Return an error view or take appropriate action
                return View("Error"); // You should create an "Error" view in your Views folder
            }
        }

        [Authorize]
        public IActionResult WatchList()
        {
            try
            {
                // Attempt to retrieve and serialize watch list coins data
                CryptoPulseHandler webHandler = new CryptoPulseHandler();
                List<Coin> watchListcoins = GetWatchList();
                string coinsData = JsonConvert.SerializeObject(watchListcoins);
                HttpContext.Session.SetString(SessionKeyName, coinsData);

                // Set the success flag to 1 on success
                ViewBag.dbSucessComp = 1;

                return View("WatchList", watchListcoins);
            }
            catch (Exception ex)
            {
                // Handle the exception here (e.g., log it or set an error flag)
                ViewBag.dbSucessComp = 0;

                // Optionally, you can pass the exception message to the view
                ViewBag.ErrorMessage = ex.Message;

                // Return an error view or take appropriate action
                return View("Error"); // You should create an "Error" view in your Views folder
            }
        }

        public IActionResult AboutUs()
        {
            try
            {
                // Your logic for successful execution here

                // Set the success flag to 1 on success
                ViewBag.dbSucessComp = 1;

                return View("AboutUs");
            }
            catch (Exception ex)
            {
                // Handle the exception here (e.g., log it or set an error flag)
                ViewBag.dbSucessComp = 0;

                // Optionally, you can pass the exception message to the view
                ViewBag.ErrorMessage = ex.Message;

                // Return an error view or take appropriate action
                return View("Error"); // You should create an "Error" view in your Views folder
            }
        }

        public IActionResult AddToWatchList(string coinJson)
        {
            //Set ViewBag variable first
            ViewBag.dbSucessComp = 0;
            // Deserialize the JSON data back to a list of Coin objects
            CoinWatchList coin = JsonConvert.DeserializeObject<CoinWatchList>(coinJson);
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    // Check if a coin with the same symbol already exists in the table
                    var existingCoin = dbContext.Coins.FirstOrDefault(c => c.Symbol.Equals(coin.Symbol) && c.IdentityUserId == _userManager.GetUserId(User));

                    if (existingCoin == null)
                    {
                        // Coin does not exist, so add it
                        // Create a new Coin object and set its properties
                        Coin newCoin = new Coin
                        {
                            IdentityUserId = _userManager.GetUserId(User),
                            Symbol = coin.Symbol,
                            Name = coin.Name,
                            Rank = coin.Rank,
                            PriceUSD = coin.PriceUSD, // Replace with the actual price
                            ID = coin.ID, // Replace with the actual ID value
                            MarketCapUSD = coin.MarketCapUSD, // Replace with the actual market cap
                            Volume24h = coin.Volume24h, // Replace with the actual 24-hour volume
                            SupplyCurrent = coin.SupplyCurrent, // Replace with the actual current supply
                            SupplyTotal = coin.SupplyTotal, // Replace with the actual total supply
                            SupplyMax = coin.SupplyMax, // Replace with the actual maximum supply
                            PercentChange1h = coin.PercentChange1h, // Replace with the actual 1-hour percentage change
                            PercentChange24h = coin.PercentChange24h, // Replace with the actual 24-hour percentage change
                            PercentChange7d = coin.PercentChange7d // Replace with the actual 7-day percentage change
                        };

                        // Add the newCoin object to the dbContext
                        dbContext.Coins.Add(newCoin);

                        // Save changes to the database
                        dbContext.SaveChanges();

                        dbContext.SaveChanges();
                        transaction.Commit();
                        ViewBag.dbSuccessComp = 1;
                    }
                    else
                    {
                        // Coin with the same symbol already exists, you can handle it as needed
                        // For example, update the existing record or skip the duplicate
                        transaction.Rollback();
                        ViewBag.dbSuccessComp = 0;
                        // Handle the duplicate coin here
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.dbSuccessComp = 0;
                    // Handle the exception as needed (e.g., log the error).
                }
            }
            CryptoPulseHandler webHandler = new CryptoPulseHandler();
            List<Coin> coins = webHandler.GetCoins();
            return View("Coins", coins);
        }

        public IActionResult DeleteFromWatchList(int coinID)
        {
            // Set ViewBag variable first
            ViewBag.dbSuccessComp = 0;
            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    // Find the coin with the given ID in the table
                    var coinToDelete = dbContext.Coins.FirstOrDefault(c => c.coinID == coinID && c.IdentityUserId == _userManager.GetUserId(User));

                    if (coinToDelete != null)
                    {
                        // Coin exists, so delete it
                        dbContext.Coins.Remove(coinToDelete);
                        dbContext.SaveChanges();

                        transaction.Commit();
                        ViewBag.dbSuccessComp = 1;
                    }
                    else
                    {
                        // Coin with the given ID does not exist, handle it as needed
                        transaction.Rollback();
                        ViewBag.dbSuccessComp = 0;
                        // Handle the case when the coin does not exist in the table
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.dbSuccessComp = 0;
                    // Handle the exception as needed (e.g., log the error).
                }
            }

            List<Coin> watchListcoins = GetWatchList();
            string coinsData = JsonConvert.SerializeObject(watchListcoins);
            HttpContext.Session.SetString(SessionKeyName, coinsData);
            return View("WatchList", watchListcoins);
        }


        public IActionResult Markets(int coinID)
        {
            try
            {
                // Attempt to retrieve and serialize market data
                CryptoPulseHandler webHandler = new CryptoPulseHandler();
                List<Market> markets = webHandler.GetMarkets(coinID);
                string marketsData = JsonConvert.SerializeObject(markets);
                HttpContext.Session.SetString(SessionKeyName, marketsData);

                // Create the view model
                var marketsViewModel = new MarketsViewModel
                {
                    Coins = webHandler.GetCoins(),
                    Markets = markets
                };

                // Set the success flag to 1 on success
                ViewBag.dbSucessComp = 1;

                return View("Markets", marketsViewModel);
            }
            catch (Exception ex)
            {
                // Handle the exception here (e.g., log it or set an error flag)
                ViewBag.dbSucessComp = 0;

                // Optionally, you can pass the exception message to the view
                ViewBag.ErrorMessage = ex.Message;

                // Return an error view or take appropriate action
                return View("Error"); // You should create an "Error" view in your Views folder
            }
        }

        public IActionResult Exchanges()
        {
            try
            {
                // Attempt to retrieve and serialize exchange data
                CryptoPulseHandler webHandler = new CryptoPulseHandler();
                List<Exchange> exchanges = webHandler.GetExchanges();
                string exchangesData = JsonConvert.SerializeObject(exchanges);
                HttpContext.Session.SetString(SessionKeyName, exchangesData);

                // Set the success flag to 1 on success
                ViewBag.dbSucessComp = 1;

                return View("Exchanges", exchanges);
            }
            catch (Exception ex)
            {
                // Handle the exception here (e.g., log it or set an error flag)
                ViewBag.dbSucessComp = 0;

                // Optionally, you can pass the exception message to the view
                ViewBag.ErrorMessage = ex.Message;

                // Return an error view or take appropriate action
                return View("Error"); // You should create an "Error" view in your Views folder
            }
        }

        public List<Coin> GetWatchList()
        {
            List<Coin> coinsForUser = new List<Coin>(); // Initialize to an empty list

            try
            {
                // Query the database to select all coins for the current user
                coinsForUser = dbContext.Coins
                    .Where(coin => coin.IdentityUserId == _userManager.GetUserId(User))
                    .ToList();

                // Set the success flag to 1 on success
                ViewBag.dbSuccessComp = 1;
            }
            catch (Exception ex)
            {
                // Set the success flag to 0 to indicate failure
                ViewBag.dbSuccessComp = 0;

                // Handle the exception as needed (e.g., log the error).
                // You can optionally store the exception message in ViewBag
                ViewBag.ErrorMessage = ex.Message;
            }

            // Return the list of coinsForUser
            return coinsForUser;
        }


        /****
         * Saves the Markets in database.
        ****/
        public IActionResult PopulateMarkets()
        {
            string marketsData = HttpContext.Session.GetString(SessionKeyName);
            List<Market> markets = null;

            using (var transaction = dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (!string.IsNullOrEmpty(marketsData))
                    {
                        markets = JsonConvert.DeserializeObject<List<Market>>(marketsData);
                    }

                    // Enable IDENTITY_INSERT for the "Markets" table
                    dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Markets ON");

                    foreach (Market market in markets)
                    {
                        // Check if the market already exists in the database based on its unique identifier.
                        // You may need to adjust the condition based on your database schema.
                        bool marketExists = dbContext.Markets.Any(m => m.MarketID == market.MarketID);

                        if (!marketExists)
                        {
                            dbContext.Markets.Add(market);
                        }
                    }

                    dbContext.SaveChanges();
                    transaction.Commit();
                    ViewBag.dbSuccessComp = 1;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    ViewBag.dbSuccessComp = 0;
                    // Handle the exception as needed (e.g., log the error).
                    // Optionally, store the exception message in ViewBag
                    ViewBag.ErrorMessage = ex.Message;
                }
                finally
                {
                    // Disable IDENTITY_INSERT
                    dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT Markets OFF");
                }
            }

            return View("Markets", markets);
        }
    }
}