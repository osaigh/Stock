using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.API.Models
{
    public class Stock
    {
        public string StockId { get; set; }
        public double Price { get; set; }
        public ICollection<StockHistory> StockHistories { get; set; }
    }
}
