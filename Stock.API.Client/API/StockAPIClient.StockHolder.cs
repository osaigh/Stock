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
        public async Task<List<StockHolder>> GetStockHolders()
        {
            List<StockHolder> stockHolders = await this.FetchAsync<List<StockHolder>>("StockHolder/");
            return stockHolders;
        }

        public async Task<StockHolder> GetStockHolder(string username)
        {
            StockHolder stockHolder = await this.FetchAsync<StockHolder>("StockHolder/" + username);
            return stockHolder;
        }

        public async Task<StockHolder> CreateStockHolder(StockHolder stockHolder)
        {
            string jsonIn = JsonConvert.SerializeObject(stockHolder);
            ApiRequest apiRequest = new ApiRequest()
                                    {
                                        JsonString = jsonIn
                                    };

            string apiRequestString = JsonConvert.SerializeObject(apiRequest);
            stockHolder = await this.PostAsync<StockHolder>("StockHolder/" ,apiRequestString);
            return stockHolder;
        }

        public async Task<StockHolder> UpdateStockHolder(StockHolder stockHolder)
        {
            string jsonIn = JsonConvert.SerializeObject(stockHolder);
            ApiRequest apiRequest = new ApiRequest()
                                    {
                                        JsonString = jsonIn
                                    };

            string apiRequestString = JsonConvert.SerializeObject(apiRequest);
            stockHolder = await this.UpdateAsync<StockHolder>("StockHolder/", apiRequestString);
            return stockHolder;
        }
    }
}
