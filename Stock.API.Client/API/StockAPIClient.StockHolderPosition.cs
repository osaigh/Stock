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
        public async Task<List<StockHolderPosition>> GetStockHolderPositions(string username)
        {
            List<StockHolderPosition> stockHolderPositions = await this.FetchAsync<List<StockHolderPosition>>("StockHolderPosition/");
            return stockHolderPositions;
        }

        public async Task<StockHolderPosition> CreateStockHolderPosition(StockHolderPosition stockHolderPosition)
        {
            string jsonIn = JsonConvert.SerializeObject(stockHolderPosition);
            ApiRequest apiRequest = new ApiRequest()
                                    {
                                        JsonString = jsonIn
                                    };

            string apiRequestString = JsonConvert.SerializeObject(apiRequest);
            stockHolderPosition = await this.PostAsync<StockHolderPosition>("StockHolderPosition/", apiRequestString);
            return stockHolderPosition;
        }

        public async Task DeleteStockHolderPosition(int id)
        {
            await this.DeleteAsync("StockHolderPosition/" + id);
        }
    }
}
