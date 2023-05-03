namespace GoldDiggerWebApi.Models
{
    public class ClassApiResponds
    {
        public int id { get; set; }
        public string metalName { get; set; } = null!;
        public decimal priceDkkPerKg { get; set; }
        public decimal priceEurPerKg { get; set; }
        public decimal priceUsdPerKg { get; set; }
    }
}
