using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.API.Models
{
    public class StockPrice
    {
        public int Id { get; set; }
        public double Price { get; set; }
        public int Volume { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
    }
}
