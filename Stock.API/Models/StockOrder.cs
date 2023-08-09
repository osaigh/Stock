using Stock.Models;
namespace Stock.API.Models
{
    public class StockOrder
    {
        public int StockOrderId { get; set; }
        public string StockId { get; set; }
        public double Shares { get; set; }
        public TimeInForce TimeInForce { get; set; }
        public TransactionType TransactionType { get; set; }
        public OrderType OrderType { get; set; }
        public double StopLimitPrice { get; set; }
    }
}
