using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.API.Models
{
    public class StockHistory
    {
        public int Id { get; set; }
        public DateTimeOffset Date { get; set; }
        public double Price { get; set; }
        public int StockId { get; set; }
        public Stock Stock { get; set; }
    }
}
