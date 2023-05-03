using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ApiCall
    {
        // The URL to the api of the open exchange rates website
        const string URL = "https://openexchangerates.org/api/latest.json?app_id=4b9528bdaf254e829c2f52f4cdaf4ad2";

        /// <summary>
        /// Returns the exchange rates from openexchangerates.org as a USDRate.
        /// It converts the json into an object using the Newtonsoft.JSON NuGet package
        /// </summary>
        /// <returns>USDRate</returns>
        public async Task<USDRate> GetJSONFromOpenExchangeRates()
        {
            // Creates a container for the data
            MemoryStream content = new MemoryStream();
            // Makes a request to openexchangerates.org
            WebRequest webReq = WebRequest.Create(URL);
            // Gets the response from the WebRequest asynchronously
            using (WebResponse response = await webReq.GetResponseAsync())
            {
                // Puts the response from the WebRequest into a new stream
                using (Stream responseStream = response.GetResponseStream())
                {
                    // Puts the result into the MemoryStream content container created earlier
                    await responseStream.CopyToAsync(content);
                }
            }
            // Converts the stream into a string
            string strContent = Encoding.UTF8.GetString(content.ToArray());
            // Converts the string (JSON) into an object (USDRate)
            return Newtonsoft.Json.JsonConvert.DeserializeObject<USDRate>(strContent);
        }
    }
}
