using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DTOStock = Stock.Models.Stock;

namespace Stock.APIClient.API
{
    public partial class StockAPIClient
    {
        public async Task<IEnumerable<DTOStock>> GetStocks()
        {
            var stockData = await this.FetchAsync<IEnumerable<DTOStock>>("Stock");

            return stockData;
        }
    }
}
