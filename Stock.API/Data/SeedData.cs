using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Stock.API.Models;

namespace Stock.API.Data
{
    public class SeedData
    {
        public static void InitializeDB(StockDbContext stockDbContext)
        {
            ////Stocks
            //if (!stockDbContext.Stocks.Any())
            //{
            //    var microsoft = new Models.Stock()
            //    {
            //        StockId = "Microsoft",
            //        Price = 89,
            //    };

            //    var slimStack = new Models.Stock()
            //    {
            //        StockId = "Slim Stack",
            //        Price = 23,
            //    };

            //    var apple = new Models.Stock()
            //    {
            //        StockId = "Apple",
            //        Price = 120,
            //    };

            //    var google = new Models.Stock()
            //    {
            //        StockId = "Google",
            //        Price = 104,
            //    };

            //    var redSpace = new Models.Stock()
            //    {
            //        StockId = "Red Space",
            //        Price = 19,
            //    };

            //    var yahoo = new Models.Stock()
            //    {
            //        StockId = "Yahoo",
            //        Price = 12,
            //    };

            //    var alliance = new Models.Stock()
            //    {
            //        StockId = "Alliance",
            //        Price = 15,
            //    };

            //    stockDbContext.Stocks.Add(microsoft);
            //    stockDbContext.Stocks.Add(slimStack);
            //    stockDbContext.Stocks.Add(apple);
            //    stockDbContext.Stocks.Add(google);
            //    stockDbContext.Stocks.Add(redSpace);
            //    stockDbContext.Stocks.Add(yahoo);
            //    stockDbContext.Stocks.Add(alliance);

            //    stockDbContext.SaveChanges();

            //    Dictionary<DateTimeOffset, double> stockHistoryValues = new Dictionary<DateTimeOffset, double>();
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 60);
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 62);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 69);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero),72);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero),80);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero),89);

            //    //Stock History
            //    //microsoft 
            //    StockHistory microsoftHistory1 = new StockHistory()
            //    {
            //        StockId = microsoft.StockId,
            //        Price = 60,
            //        Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory microsoftHistory2 = new StockHistory()
            //    {
            //        StockId = microsoft.StockId,
            //        Price = 62,
            //        Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory microsoftHistory3 = new StockHistory()
            //    {
            //        StockId = microsoft.StockId,
            //        Price = 69,
            //        Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory microsoftHistory4 = new StockHistory()
            //    {
            //        StockId = microsoft.StockId,
            //        Price = 72,
            //        Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory microsoftHistory5 = new StockHistory()
            //    {
            //        StockId = microsoft.StockId,
            //        Price = 80,
            //        Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory microsoftHistory6 = new StockHistory()
            //    {
            //        StockId = microsoft.StockId,
            //        Price = 89,
            //        Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    stockDbContext.StockHistories.Add(microsoftHistory1);
            //    stockDbContext.StockHistories.Add(microsoftHistory2);
            //    stockDbContext.StockHistories.Add(microsoftHistory3);
            //    stockDbContext.StockHistories.Add(microsoftHistory4);
            //    stockDbContext.StockHistories.Add(microsoftHistory5);
            //    stockDbContext.StockHistories.Add(microsoftHistory6);

            //    //slimStack 
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero),20);
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero),26);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero),33);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero),29);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero),19);
            //    stockHistoryValues.Add(new DateTimeOffset(2024, 4, 30, 0, 0, 0, TimeSpan.Zero),23);
            //    StockHistory slimStackHistory1 = new StockHistory()
            //    {
            //        StockId = slimStack.StockId,
            //        Price = 20,
            //        Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory slimStackHistory2 = new StockHistory()
            //    {
            //        StockId = slimStack.StockId,
            //        Price = 26,
            //        Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory slimStackHistory3 = new StockHistory()
            //    {
            //        StockId = slimStack.StockId,
            //        Price = 33,
            //        Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory slimStackHistory4 = new StockHistory()
            //    {
            //        StockId = slimStack.StockId,
            //        Price = 29,
            //        Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory slimStackHistory5 = new StockHistory()
            //    {
            //        StockId = slimStack.StockId,
            //        Price = 19,
            //        Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory slimStackHistory6 = new StockHistory()
            //    {
            //        StockId = slimStack.StockId,
            //        Price = 23,
            //        Date = new DateTimeOffset(2024, 4, 30, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    stockDbContext.StockHistories.Add(slimStackHistory1);
            //    stockDbContext.StockHistories.Add(slimStackHistory2);
            //    stockDbContext.StockHistories.Add(slimStackHistory3);
            //    stockDbContext.StockHistories.Add(slimStackHistory4);
            //    stockDbContext.StockHistories.Add(slimStackHistory5);
            //    stockDbContext.StockHistories.Add(slimStackHistory6);

            //    //apple 
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero),80);
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero),85);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero),91);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero),100);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero),110);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero),120);
            //    StockHistory appleHistory1 = new StockHistory()
            //    {
            //        StockId = apple.StockId,
            //        Price = 80,
            //        Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory appleHistory2 = new StockHistory()
            //    {
            //        StockId = apple.StockId,
            //        Price = 85,
            //        Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory appleHistory3 = new StockHistory()
            //    {
            //        StockId = apple.StockId,
            //        Price = 91,
            //        Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory appleHistory4 = new StockHistory()
            //    {
            //        StockId = apple.StockId,
            //        Price = 100,
            //        Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    StockHistory appleHistory5 = new StockHistory()
            //    {
            //        StockId = apple.StockId,
            //        Price = 110,
            //        Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    StockHistory appleHistory6 = new StockHistory()
            //    {
            //        StockId = apple.StockId,
            //        Price = 120,
            //        Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    stockDbContext.StockHistories.Add(appleHistory1);
            //    stockDbContext.StockHistories.Add(appleHistory2);
            //    stockDbContext.StockHistories.Add(appleHistory3);
            //    stockDbContext.StockHistories.Add(appleHistory4);
            //    stockDbContext.StockHistories.Add(appleHistory5);
            //    stockDbContext.StockHistories.Add(appleHistory6);

            //    //google 
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 70);
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 76);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 82);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 93);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 100);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 104);
            //    StockHistory googleHistory1 = new StockHistory()
            //    {
            //        StockId = google.StockId,
            //        Price = 70,
            //        Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory googleHistory2 = new StockHistory()
            //    {
            //        StockId = google.StockId,
            //        Price = 76,
            //        Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory googleHistory3 = new StockHistory()
            //    {
            //        StockId = google.StockId,
            //        Price = 82,
            //        Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory googleHistory4 = new StockHistory()
            //    {
            //        StockId = google.StockId,
            //        Price = 93,
            //        Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    StockHistory googleHistory5 = new StockHistory()
            //    {
            //        StockId = google.StockId,
            //        Price = 100,
            //        Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    StockHistory googleHistory6 = new StockHistory()
            //    {
            //        StockId = google.StockId,
            //        Price = 104,
            //        Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    stockDbContext.StockHistories.Add(googleHistory1);
            //    stockDbContext.StockHistories.Add(googleHistory2);
            //    stockDbContext.StockHistories.Add(googleHistory3);
            //    stockDbContext.StockHistories.Add(googleHistory4);
            //    stockDbContext.StockHistories.Add(googleHistory5);
            //    stockDbContext.StockHistories.Add(googleHistory6);

            //    //redSpace 
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 9);
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 13);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 15);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 18);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 23);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 19);
            //    StockHistory redSpaceHistory1 = new StockHistory()
            //    {
            //        StockId = redSpace.StockId,
            //        Price = 9,
            //        Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory redSpaceHistory2 = new StockHistory()
            //    {
            //        StockId = redSpace.StockId,
            //        Price = 13,
            //        Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory redSpaceHistory3 = new StockHistory()
            //    {
            //        StockId = redSpace.StockId,
            //        Price = 15,
            //        Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory redSpaceHistory4 = new StockHistory()
            //    {
            //        StockId = redSpace.StockId,
            //        Price = 18,
            //        Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    StockHistory redSpaceHistory5 = new StockHistory()
            //    {
            //        StockId = redSpace.StockId,
            //        Price = 23,
            //        Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    StockHistory redSpaceHistory6 = new StockHistory()
            //    {
            //        StockId = redSpace.StockId,
            //        Price = 19,
            //        Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    stockDbContext.StockHistories.Add(redSpaceHistory1);
            //    stockDbContext.StockHistories.Add(redSpaceHistory2);
            //    stockDbContext.StockHistories.Add(redSpaceHistory3);
            //    stockDbContext.StockHistories.Add(redSpaceHistory4);
            //    stockDbContext.StockHistories.Add(redSpaceHistory5);
            //    stockDbContext.StockHistories.Add(redSpaceHistory6);

            //    //yahoo 
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 79);
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 76);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 56);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 38);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 26);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 12);
            //    StockHistory yahooHistory1 = new StockHistory()
            //    {
            //        StockId = yahoo.StockId,
            //        Price = 79,
            //        Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory yahooHistory2 = new StockHistory()
            //    {
            //        StockId = yahoo.StockId,
            //        Price = 76,
            //        Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory yahooHistory3 = new StockHistory()
            //    {
            //        StockId = yahoo.StockId,
            //        Price = 56,
            //        Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory yahooHistory4 = new StockHistory()
            //    {
            //        StockId = yahoo.StockId,
            //        Price = 38,
            //        Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    StockHistory yahooHistory5 = new StockHistory()
            //    {
            //        StockId = yahoo.StockId,
            //        Price = 26,
            //        Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    StockHistory yahooHistory6 = new StockHistory()
            //    {
            //        StockId = yahoo.StockId,
            //        Price = 12,
            //        Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    stockDbContext.StockHistories.Add(yahooHistory1);
            //    stockDbContext.StockHistories.Add(yahooHistory2);
            //    stockDbContext.StockHistories.Add(yahooHistory3);
            //    stockDbContext.StockHistories.Add(yahooHistory4);
            //    stockDbContext.StockHistories.Add(yahooHistory5);
            //    stockDbContext.StockHistories.Add(yahooHistory6);

            //    //alliance 
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 14);
            //    stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 18);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 20);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 22);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 18);
            //    stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 15);
            //    StockHistory allianceHistory1 = new StockHistory()
            //    {
            //        StockId = alliance.StockId,
            //        Price = 14,
            //        Date = new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory allianceHistory2 = new StockHistory()
            //    {
            //        StockId = alliance.StockId,
            //        Price = 18,
            //        Date = new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory allianceHistory3 = new StockHistory()
            //    {
            //        StockId = alliance.StockId,
            //        Price = 20,
            //        Date = new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero)
            //    };
            //    StockHistory allianceHistory4 = new StockHistory()
            //    {
            //        StockId = alliance.StockId,
            //        Price = 22,
            //        Date = new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    StockHistory allianceHistory5 = new StockHistory()
            //    {
            //        StockId = alliance.StockId,
            //        Price = 18,
            //        Date = new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    StockHistory allianceHistory6 = new StockHistory()
            //    {
            //        StockId = alliance.StockId,
            //        Price = 15,
            //        Date = new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero)
            //    };

            //    stockDbContext.StockHistories.Add(allianceHistory1);
            //    stockDbContext.StockHistories.Add(allianceHistory2);
            //    stockDbContext.StockHistories.Add(allianceHistory3);
            //    stockDbContext.StockHistories.Add(allianceHistory4);
            //    stockDbContext.StockHistories.Add(allianceHistory5);
            //    stockDbContext.StockHistories.Add(allianceHistory6);

            //    stockDbContext.SaveChanges();
            //}
            AddStocks(stockDbContext);
        }

        private static void AddStocks(StockDbContext stockDbContext)
        {
            //Stocks
            if (!stockDbContext.Stocks.Any())
            {
                var microsoft = new Models.Stock()
                {
                    StockId = "Microsoft",
                    Price = 89,
                };

                var slimStack = new Models.Stock()
                {
                    StockId = "Slim Stack",
                    Price = 23,
                };

                var apple = new Models.Stock()
                {
                    StockId = "Apple",
                    Price = 120,
                };

                var google = new Models.Stock()
                {
                    StockId = "Google",
                    Price = 104,
                };

                var redSpace = new Models.Stock()
                {
                    StockId = "Red Space",
                    Price = 19,
                };

                var yahoo = new Models.Stock()
                {
                    StockId = "Yahoo",
                    Price = 12,
                };

                var alliance = new Models.Stock()
                {
                    StockId = "Alliance",
                    Price = 15,
                };

                Dictionary<DateTimeOffset, double> stockHistoryValues = new Dictionary<DateTimeOffset, double>();

                stockDbContext.Stocks.Add(microsoft);
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 60);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 62);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 69);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 72);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 80);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 89);
                AddStockHistory(stockDbContext, microsoft, stockHistoryValues);
                AddStockPosition(stockDbContext, microsoft,200,17800,1);

                stockDbContext.Stocks.Add(slimStack);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 20);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 26);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 33);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 29);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 19);
                stockHistoryValues.Add(new DateTimeOffset(2024, 4, 30, 0, 0, 0, TimeSpan.Zero), 23);
                AddStockHistory(stockDbContext, slimStack, stockHistoryValues);
                AddStockPosition(stockDbContext, slimStack, 10, 230,2);

                stockDbContext.Stocks.Add(apple);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 80);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 85);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 91);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 100);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 110);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 120);
                AddStockHistory(stockDbContext, apple, stockHistoryValues);
                AddStockPosition(stockDbContext, apple,100,12000,3);

                stockDbContext.Stocks.Add(google);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 70);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 76);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 82);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 93);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 100);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 104);
                AddStockHistory(stockDbContext, google, stockHistoryValues);
                AddStockPosition(stockDbContext, google,200,20000,4);

                stockDbContext.Stocks.Add(redSpace);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 9);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 13);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 15);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 18);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 23);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 19);
                AddStockHistory(stockDbContext, redSpace, stockHistoryValues);
                AddStockPosition(stockDbContext, redSpace, 40, 72,5);

                stockDbContext.Stocks.Add(yahoo);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 79);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 76);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 56);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 38);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 26);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 12);
                AddStockHistory(stockDbContext, yahoo, stockHistoryValues);
                AddStockPosition(stockDbContext, yahoo,30, 3600,6);

                stockDbContext.Stocks.Add(alliance);
                stockHistoryValues.Clear();
                stockHistoryValues.Add(new DateTimeOffset(2020, 11, 30, 0, 0, 0, TimeSpan.Zero), 14);
                stockHistoryValues.Add(new DateTimeOffset(2020, 12, 31, 0, 0, 0, TimeSpan.Zero), 18);
                stockHistoryValues.Add(new DateTimeOffset(2021, 1, 31, 0, 0, 0, TimeSpan.Zero), 20);
                stockHistoryValues.Add(new DateTimeOffset(2021, 2, 27, 0, 0, 0, TimeSpan.Zero), 22);
                stockHistoryValues.Add(new DateTimeOffset(2021, 3, 31, 0, 0, 0, TimeSpan.Zero), 18);
                stockHistoryValues.Add(new DateTimeOffset(2021, 4, 30, 0, 0, 0, TimeSpan.Zero), 15);
                AddStockHistory(stockDbContext, alliance, stockHistoryValues);
                AddStockPosition(stockDbContext, alliance,500,10000,7);

                stockDbContext.SaveChanges();
            }

        }

        private static void AddStockHistory(StockDbContext stockDbContext, Models.Stock stock, Dictionary<DateTimeOffset,double> stockHistoryValues)
        {
            foreach(var stockHistoryValue in stockHistoryValues)
            {
                StockHistory stockHistory = new StockHistory()
                {
                    StockId = stock.StockId,
                    Price = stockHistoryValue.Value,
                    Date = stockHistoryValue.Key
                };

                stockDbContext.StockHistories.Add(stockHistory);
            }
            stockDbContext.SaveChanges();
        }

        private static void AddStockPosition(StockDbContext stockDbContext, Models.Stock stock, int shares, double costBasis, int stockPositionId)
        {
            StockPosition stockPosition = new StockPosition()
            {
                StockPositionId= stockPositionId,
                StockId = stock.StockId,
                Shares = shares,
                CostBasis = costBasis,

            };

            stockDbContext.StockPositions.Add(stockPosition);
            stockDbContext.SaveChanges();
        }
    }
}
