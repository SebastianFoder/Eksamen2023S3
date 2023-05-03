using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GoldDiggerWebApi.Models;

namespace GoldDiggerWebApi.Controllers
{
    public class WebController : Controller
    {
        private readonly GoldDiggerDbContext _context;

        public WebController(GoldDiggerDbContext context)
        {
            _context = context;
        }

        // GET: Web
        public async Task<IActionResult> Index()
        {
            List<ClassApiResponds> list = new List<ClassApiResponds>();
            using (GoldDiggerDbContext dbContext = new GoldDiggerDbContext()) // Makes a new context for of the database
            {
                if (dbContext.PreciousMetals == null || dbContext.Rates == null) // returns an error if the database is empty
                {
                    return NotFound();
                }

                IEnumerable<Rate> rates = from rate in await dbContext.Rates.ToListAsync() // Gets the required rates from the database
                                          where rate.Code == "DKK" || rate.Code == "EUR"
                                          select rate;
                var dkkRate = (from rate in rates // Gets the dkk rate from the list
                               where rate.Code == "DKK"
                               select rate.Rates).First();
                var eurRate = (from rate in rates // Gets the eur rate from the list
                               where rate.Code == "EUR"
                               select rate.Rates).First();


                // Runs thru all the metal in the database and converts them to a ClassApiResponds
                // and calculated their price in the diffrent valutas
                foreach (var item in await dbContext.PreciousMetals.ToListAsync())
                {
                    list.Add(new ClassApiResponds
                    {
                        id = item.Id,
                        metalName = item.MetalName,
                        priceDkkPerKg = item.PricePerKg,
                        priceEurPerKg = (item.PricePerKg / dkkRate) * eurRate,
                        priceUsdPerKg = item.PricePerKg / dkkRate
                    });
                }
            }

            return View(list);
        }
    }
}
