using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Models;

namespace Stock.API.Repository
{
    public class StockPositionRepository: IRepository<StockPosition>
    {
        #region fields
        private readonly StockDbContext _StockDbContext;
        #endregion

        #region Constructor
        public StockPositionRepository(StockDbContext stockDbContext)
        {
            _StockDbContext = stockDbContext;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<StockPosition> AddAsync(StockPosition stockPosition)
        {
            if (string.IsNullOrEmpty(stockPosition.StockId)
                || stockPosition.Shares <= 0)
            {
                string message = string.IsNullOrEmpty(stockPosition.StockId) ? "StockId is invalid" : (stockPosition.Shares <= 0 ? "Shares is invalid" : "invalid request");
                throw new ArgumentException(message);
            }

            //Get the stock if it exist
            var stock = await _StockDbContext.Stocks
                                             .FirstOrDefaultAsync(s => s.StockId == stockPosition.StockId);
            if (stock == null)
            {
                return null;
            }

       
            //check to see if this Stock position already exist
            var _stockPosition = await _StockDbContext.StockPositions
                                                            .FirstOrDefaultAsync(s => s.StockId == stockPosition.StockId);
            if (_stockPosition != null)
            {
                throw new Exception("Position already exist");
            }

            stockPosition.CostBasis = stock.Price * stockPosition.Shares;
            await _StockDbContext.StockPositions.AddAsync(stockPosition);

            await _StockDbContext.SaveChangesAsync();

            return stockPosition;
        }

        public async Task DeleteAsync(StockPosition stockPosition)
        {
            if (string.IsNullOrEmpty(stockPosition.StockId)
                || stockPosition.Shares <= 0)
            {
                string message =  "StockId is invalid";
                throw new ArgumentException(message);
            }

            //check to see if this Stock position already exist
            var _stockPosition = await _StockDbContext.StockPositions
                                                            .FirstOrDefaultAsync(s => s.StockPositionId == stockPosition.StockPositionId);
            if (_stockPosition != null)
            {
                _StockDbContext.StockPositions.Remove(_stockPosition);
            }

            await _StockDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<StockPosition>> SearchForAsync(Expression<Func<StockPosition, bool>> predicate)
        {
            var stockPositions = await _StockDbContext.StockPositions.Include(sp => sp.Stock)
                                        .Where(predicate).ToListAsync();

            foreach (var stockPosition in stockPositions)
            {
                stockPosition.CurrentPrice = stockPosition.Stock.Price;
            }

            return stockPositions;
        }

        public async Task<IEnumerable<StockPosition>> GetAllAsync()
        {
            var stockPositions = await _StockDbContext.StockPositions.Include(sp => sp.Stock)
                                        .ToListAsync();

            foreach(var stockPosition in stockPositions)
            {
                stockPosition.CurrentPrice = stockPosition.Stock.Price;
            }

            return stockPositions;

        }

        public async Task<StockPosition> GetAsync(object id)
        {
            int key = int.Parse(id.ToString());
            var stockPosition = await _StockDbContext.StockPositions.Include(sp => sp.Stock)
                                        .FirstOrDefaultAsync(s => s.StockPositionId == key);

            if (stockPosition != null)
            {
                stockPosition.CurrentPrice = stockPosition.Stock.Price;
            }

            return stockPosition;
        }

        public async Task<StockPosition> UpdateAsync(StockPosition stockPosition)
        {
            var _stockPosition = await GetAsync(stockPosition.StockPositionId);


            if (_stockPosition == null)
            {
                return null;
            }

            if (!ReferenceEquals(stockPosition, _stockPosition))
            {
                _StockDbContext.Entry(_stockPosition).CurrentValues.SetValues(stockPosition);
            }

            await _StockDbContext.SaveChangesAsync();

            return _stockPosition;
        }
        #endregion
    }
}
