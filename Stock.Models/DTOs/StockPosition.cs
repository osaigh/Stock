using System;
using System.Collections.Generic;
using System.Text;

namespace Stock.Models
{
    public class StockPosition
    {
        public int StockPositionId { get; set; }
        public double CostBasis { get; set; }
        public int Shares { get; set; }
        public string StockId { get; set; }
        public double CurrentPrice { get; set; }
        public double MarketValue { get { return Math.Round(Shares * CurrentPrice,2); } }
        public double GainLossPercent { get { return Math.Round(((CurrentPrice * Shares - CostBasis) * 100 / CostBasis),2); } }
    }
}
