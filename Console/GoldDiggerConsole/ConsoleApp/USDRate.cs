using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    /// <summary>
    /// Used by Newtonsofts JsonDeserialiser to insert all the json data from OpenExchangeRates.org into a C# object
    /// In other words, everything recieved from OpenExchangeRates.org will be put into this object
    /// </summary>
    public class USDRate
    {
        public string Disclaimer { get; set; }

        public string License { get; set; }

        public long Timestamp { get; set; }

        public string Base { get; set; }

        public Dictionary<string, decimal> Rates { get; set; }
    }
}
