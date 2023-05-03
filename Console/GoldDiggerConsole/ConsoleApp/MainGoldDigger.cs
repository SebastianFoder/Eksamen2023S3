using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    internal class MainGoldDigger
    {
        static private ApiCall apiCall;
        static private USDRate usdRate;
        static private DbCall dbCall;

        static void Main(string[] args)
        {
            // This class appears to never be instantiated,
            // so everything that you would put in a constructor must be put here instead
            apiCall = new ApiCall();
            usdRate = new USDRate();
            dbCall = new DbCall();

            Console.WriteLine("Starting program...");
            // This method isn't async and needs to use .Wait() to run an async method
            StartTimer().Wait();
        }

        /// <summary>
        /// Starts the timer that decides when to fetch and update exchange rates
        /// </summary>
        /// <returns></returns>
        static private async Task StartTimer()
        {
            while (true)
            {
                var delay = 60 - DateTime.Now.Second;
                //Output feedback to the console
                await Console.Out.WriteLineAsync($"Will Update in the next {delay} seconds");
                await Task.Delay(delay * 1000);

                await GetDollarRateFromAPI();
                await UpdateDatabaseWithDollarRate();
                await Console.Out.WriteLineAsync();
            }
        }

        /// <summary>
        /// Uses the api to get the exchange rates for USD
        /// </summary>
        static private async Task GetDollarRateFromAPI()
        {
            usdRate = await apiCall.GetJSONFromOpenExchangeRates();
            //Output feedback to the console
            await Console.Out.WriteLineAsync("Succesfully retrieved rates from openexchangerates.org");
        }

        /// <summary>
        /// Updates the exchange rates in the database with the result from the GetDollarRateFromAPI method
        /// </summary>
        static private async Task UpdateDatabaseWithDollarRate()
        {
            await dbCall.UpdateRateInDB(usdRate);
            //Output feedback to the console
            await Console.Out.WriteLineAsync("Succesfully inserted rates in database");
        }
    }
}
