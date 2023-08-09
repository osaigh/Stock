namespace Stock.API.Models
{
    public class StockPosition
    {
        public int StockPositionId { get; set; }
        public string StockId { get; set; }
        public Stock Stock { get; set; }
        public int Shares { get; set; }
        public double CurrentPrice { get; set; }
        public double CostBasis { get; set; }

    }
}
