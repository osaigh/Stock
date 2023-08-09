using Stock.API.Data;
using Stock.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Stock.API.Repository
{
    public class StockOrderRepository : IRepository<StockOrder>
    {
        #region Fields
        private readonly StockDbContext _StockDbContext;
        private readonly IMapper _Mapper;
        #endregion

        #region Fields
        public StockOrderRepository(StockDbContext stockDbContext, IMapper mapper)
        {
            _StockDbContext = stockDbContext;
            _Mapper = mapper;
        }
        #endregion

        #region IRepository
        public async Task<StockOrder> AddAsync(StockOrder stockOrder)
        {
            var stock = await _StockDbContext.Stocks.FirstOrDefaultAsync(s => s.StockId == stockOrder.StockId);

            if (stock == null)
            {
                return null;
            }

            stockOrder.StockOrderId = await GetUniqueId();
            await _StockDbContext.StockOrders.AddAsync(stockOrder);
            await _StockDbContext.SaveChangesAsync();

            return stockOrder;
        }

        public async Task DeleteAsync(StockOrder stockOrder)
        {
            var _stockOrder = await GetAsync(stockOrder.StockOrderId);
            if (_stockOrder != null)
            {
                _StockDbContext.StockOrders.Remove(_stockOrder);
                await _StockDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<StockOrder>> GetAllAsync()
        {
            return await _StockDbContext.StockOrders
                                        .ToListAsync();
        }

        public async Task<StockOrder> GetAsync(object id)
        {
            int key = int.Parse(id.ToString());
            return await _StockDbContext.StockOrders
                                         .FirstOrDefaultAsync(s => s.StockOrderId == key);
        }

        public async Task<IEnumerable<StockOrder>> SearchForAsync(Expression<Func<StockOrder, bool>> predicate)
        {
            return await _StockDbContext.StockOrders
                                        .Where(predicate).ToListAsync();
        }

        public async Task<StockOrder> UpdateAsync(StockOrder stockOrder)
        {
            var _stockOrder = await GetAsync(stockOrder.StockOrderId);

            if (_stockOrder == null)
            {
                return null;
            }

            if (!ReferenceEquals(stockOrder, _stockOrder))
            {
                _Mapper.Map(stockOrder, _stockOrder);
            }

            await _StockDbContext.SaveChangesAsync();

            return _stockOrder;
        }

        private async Task<int> GetUniqueId()
        {
            var stockOrders = await this.GetAllAsync();
            int id = 0;
            foreach (var stockOrder in stockOrders)
            {
                if(stockOrder.StockOrderId > id)
                {
                    id = stockOrder.StockOrderId;
                }
            };

            return id + 1;
        }
        #endregion
    }
}
