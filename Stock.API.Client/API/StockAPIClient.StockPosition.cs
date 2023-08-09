using Stock.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Stock.APIClient.API
{
    public partial class StockAPIClient
    {
        public async Task<List<StockPosition>> GetStockPositions(string accessToken = null)
        {
            List<StockPosition> stockPositions = await this.FetchAsync<List<StockPosition>>("StockPosition/", accessToken);
            return stockPositions;
        }

       
    }
}
