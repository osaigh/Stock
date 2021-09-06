using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.API.Models
{
    public class StockHolder
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public ICollection<StockHolderPosition> StockHolderPositions { get; set; }

    }
}
