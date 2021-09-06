using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.API.Models
{
    public class Stock
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Volume { get; set; }
        public ICollection<StockHistory> StockHistories { get; set; }
    }
}
