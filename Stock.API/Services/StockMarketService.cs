﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.API.Data;


namespace Stock.API.Services
{
    public class StockMarketService: IStockMarketService
    {
        #region Fields
        private readonly Random _random = new Random(((int)DateTime.Now.Ticks/1000));
        private readonly StockDbContext _StockDbContext;
        #endregion

        #region Constructor
        public StockMarketService(StockDbContext stockDbContext)
        {
            _StockDbContext = stockDbContext;
        }
        #endregion

        #region Methods
        public async Task UpdateStockPrices()
        {
            var stocks = await _StockDbContext.Stocks.ToListAsync();
            foreach (var stock in stocks)
            {
                double newPrice = stock.Price + Convert.ToDouble((Convert.ToDecimal(_random.NextDouble() * 10f) - 5m)) ;
                stock.Price = newPrice > 0 ? newPrice : 1;
            }

            await _StockDbContext.SaveChangesAsync();
        }

        #endregion

    }
}
