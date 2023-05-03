using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Repository;

namespace IO
{
    public class ClassCallWebApi
    {

        public ClassCallWebApi()
        {

        }

        /// <summary>
        /// Asynchronously retrieves the contents of a GET Request to our API that returns our metals in JSON format.
        /// The method sends an HTTP request to the URL and receives the response in a stream, which is then read 
        /// into a memory stream using the CopyToAsync method.
        /// The memory stream content is then converted to a string using the UTF8 encoding and deserialized into a List of ClassApiResponds objects
        /// using the JsonConvert.DeserializeObject method from the Newtonsoft.Json library.
        /// </summary>
        /// <returns>Returns a List of ClassApiResponds objects.</returns>
        public async Task<List<ClassApiResponds>> GetURLContentsAsync()
        {
            string res = "";
            var content = new MemoryStream();
            var webReq = (HttpWebRequest)WebRequest.Create("https://localhost:44370/api/golddigger");

            try
            {
                using (WebResponse response = await webReq.GetResponseAsync())
                {
                    using (Stream responseStream = response.GetResponseStream())
                    {
                        await responseStream.CopyToAsync(content);
                    }
                }
                res = System.Text.Encoding.UTF8.GetString(content.ToArray());
            }
            catch (IOException ex)
            {
                throw ex;
            }
            finally
            {
                content.Close();
            }

            return JsonConvert.DeserializeObject<List<ClassApiResponds>>(res);
        }

    }
}
