using System;
using System.Collections.Generic;
using Stock.API.Data;
using Stock.API.Models;
using OrderType = Stock.Models.OrderType;
using TimeInForce = Stock.Models.TimeInForce;
using TransactionType = Stock.Models.TransactionType;

namespace Stock.API.Tests
{
    public static class Utilities
    {
        public const string Microsoft = "Microsoft";
        public const string Google = "Google";

        public static void InitializeDbForTests(StockDbContext db)
        {
            db.Stocks.AddRange(GetTestStocks());
            db.StockHistories.AddRange(GetTestStockHistories());
            db.StockPositions.AddRange(GetTestStockPositions());
            db.StockOrders.AddRange(GetTestStockOrders());

            db.SaveChanges();
        }

        public static List<Models.Stock> GetTestStocks()
        {
            return new List<Models.Stock>()
                   {
                       new Models.Stock() { StockId = Microsoft, Price = 85 },
                       new Models.Stock() { StockId = Google, Price = 105 },
                   };
        }

        /// <summary>
        /// The histories are deliberately not in date order, so the tests can check that the API sorts them
        /// </summary>
        public static List<StockHistory> GetTestStockHistories()
        {
            return new List<StockHistory>()
                   {
                       new StockHistory() { StockHistoryId = 1, StockId = Microsoft, Date = new DateTimeOffset(2020, 12, 2, 0, 0, 0, TimeSpan.Zero), Price = 70 },
                       new StockHistory() { StockHistoryId = 2, StockId = Microsoft, Date = new DateTimeOffset(2018, 12, 2, 0, 0, 0, TimeSpan.Zero), Price = 45 },
                       new StockHistory() { StockHistoryId = 3, StockId = Microsoft, Date = new DateTimeOffset(2019, 12, 2, 0, 0, 0, TimeSpan.Zero), Price = 65 },
                       new StockHistory() { StockHistoryId = 4, StockId = Google, Date = new DateTimeOffset(2019, 12, 2, 0, 0, 0, TimeSpan.Zero), Price = 100 },
                       new StockHistory() { StockHistoryId = 5, StockId = Google, Date = new DateTimeOffset(2020, 12, 2, 0, 0, 0, TimeSpan.Zero), Price = 120 },
                   };
        }

        public static List<StockPosition> GetTestStockPositions()
        {
            return new List<StockPosition>()
                   {
                       new StockPosition() { StockPositionId = 1, StockId = Microsoft, Shares = 10, CostBasis = 800 },
                       new StockPosition() { StockPositionId = 2, StockId = Google, Shares = 20, CostBasis = 2000 },
                   };
        }

        public static List<StockOrder> GetTestStockOrders()
        {
            return new List<StockOrder>()
                   {
                       new StockOrder()
                       {
                           StockOrderId = 1,
                           StockId = Microsoft,
                           Shares = 5,
                           OrderType = OrderType.Limit,
                           TransactionType = TransactionType.Buy,
                           TimeInForce = TimeInForce.ThirtyDays,
                           StopLimitPrice = 80,
                       },
                       new StockOrder()
                       {
                           StockOrderId = 2,
                           StockId = Google,
                           Shares = 3,
                           OrderType = OrderType.Market,
                           TransactionType = TransactionType.Sell,
                           TimeInForce = TimeInForce.EndOfDay,
                           StopLimitPrice = 110,
                       },
                   };
        }
    }
}
