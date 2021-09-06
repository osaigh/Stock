using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stock.Models;

namespace Stock.APIClient.API
{
    public partial class StockAPIClient
    {
        public async Task UpdateStockMarket()
        {
            ApiRequest apiRequest = new ApiRequest() { };

            string apiRequestString = JsonConvert.SerializeObject(apiRequest);
            await this.UpdateAsync("StockMarket/", apiRequestString);
        }
    }
}
