using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Stock.Models;

namespace Stock.APIClient.API
{
    public partial class StockAPIClient
    {
        public async Task<List<StockHistory>> GetStockHistories()
        {
            List<StockHistory> stockHistories = await this.FetchAsync<List<StockHistory>>("StockHistory/");
            return stockHistories;
        }

        public async Task<StockHistory> GetStockHistory(int stockId)
        {
            StockHistory stockHistory = await this.FetchAsync<StockHistory>("StockHistory/" + stockId);
            return stockHistory;
        }
    }
}
