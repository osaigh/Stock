using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stock.Models;

namespace Stock.APIClient.API
{
    public partial class StockAPIClient
    {
        public async Task<List<StockPositionSummary>> GetStockPrices()
        {
            List<StockPositionSummary> stockPrices = await this.FetchAsync<List<StockPositionSummary>>("StockPrice/");
            return stockPrices;
        }

        public async Task<StockPositionSummary> GetStockPrice(int id)
        {
            StockPositionSummary stockPrice = await this.FetchAsync<StockPositionSummary>("StockPrice/"+ id);
            return stockPrice;
        }
    }
}
