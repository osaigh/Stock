﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Models;

namespace Stock.API.Repository
{
    public class StockHolderRepository: IRepository<StockHolder> 
    {
        #region fields
        private readonly StockDbContext _StockDbContext;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockHolderRepository(StockDbContext stockDbContext, IMapper mapper)
        {
            _StockDbContext = stockDbContext;
            _Mapper = mapper;
        }
        #endregion

        #region Methods

        #endregion

        #region IRepository

        public async Task<StockHolder> AddAsync(StockHolder stockHolder)
        {
            await _StockDbContext.StockHolders.AddAsync(stockHolder);
            await _StockDbContext.SaveChangesAsync();

            return stockHolder;
        }

        public async Task DeleteAsync(StockHolder stockHolder)
        {
            var _stockHolder = await GetAsync(stockHolder.Username);
            if (_stockHolder != null)
            {
                _StockDbContext.StockHolders.Remove(_stockHolder);
                await _StockDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<StockHolder>> SearchForAsync(Expression<Func<StockHolder, bool>> predicate)
        {
            return await _StockDbContext.StockHolders
                                        .Include(s => s.StockHolderPositions)
                                        .ThenInclude(s => s.Stock)
                                        .Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<StockHolder>> GetAllAsync()
        {
            return await _StockDbContext.StockHolders
                                        .Include(s => s.StockHolderPositions)
                                        .ThenInclude(s => s.Stock)
                                        .ToListAsync();
        }

        public async Task<StockHolder> GetAsync(object id)
        {
            return  await _StockDbContext.StockHolders
                                         .Include(s => s.StockHolderPositions)
                                         .ThenInclude(s => s.Stock)
                                         .FirstOrDefaultAsync(s => s.Username.ToLower() == id.ToString().ToLower());
        }

        public async Task<StockHolder> UpdateAsync(StockHolder stockHolder)
        {
            var _stockHolder = await GetAsync(stockHolder.Username);

            if (_stockHolder == null)
            {
                return null;
            }

            if (!ReferenceEquals(stockHolder, _stockHolder))
            {
                _Mapper.Map(stockHolder, _stockHolder);
            }

            await _StockDbContext.SaveChangesAsync();

            return _stockHolder;
        }

        #endregion
    }
}
