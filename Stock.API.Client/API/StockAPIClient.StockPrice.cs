using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stock.Models;

namespace Stock.APIClient.API
{
    public partial class StockAPIClient
    {
        public async Task<List<StockPrice>> GetStockPrices()
        {
            List<StockPrice> stockPrices = await this.FetchAsync<List<StockPrice>>("StockPrice/");
            return stockPrices;
        }

        public async Task<StockPrice> GetStockPrice(int id)
        {
            StockPrice stockPrice = await this.FetchAsync<StockPrice>("StockPrice/"+ id);
            return stockPrice;
        }
    }
}
