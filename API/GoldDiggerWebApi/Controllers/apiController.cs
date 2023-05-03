using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GoldDiggerWebApi.Models;

namespace GoldDiggerWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GoldDiggerController : ControllerBase
    {
        private readonly GoldDiggerDbContext _context;

        public GoldDiggerController(GoldDiggerDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Communicated with the database and returns
        /// All the required metals
        /// </summary>
        /// <returns>List of metal with the prices in diffrent valutas</returns>
        // GET: api/golddigger
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ClassApiResponds>>> golddigger()
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
                    list.Add(new ClassApiResponds { id = item.Id, metalName = item.MetalName, priceDkkPerKg = item.PricePerKg,
                    priceEurPerKg = (item.PricePerKg / dkkRate) * eurRate, priceUsdPerKg = item.PricePerKg / dkkRate 
                    });
                }
            }
            return list;
        }
    }
}
