namespace IO
{
    /// <summary>
    /// Holds data from the api response
    /// </summary>
    public class ClassApiResponds
    {
        public int id { get; set; }
        public string metalName { get; set; }
        public decimal priceDkkPerKg { get; set; }
        public decimal priceEurPerKg { get; set; }
        public decimal priceUsdPerKg { get; set; }
    }
}
