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
        public async Task<List<StockOrder>> GetStockOrders()
        {
            List<StockOrder> stockOrders = await this.FetchAsync<List<StockOrder>>("StockOrder/");
            return stockOrders;
        }

        public async Task<StockOrder> GetStockOrder(string stockId)
        {
            StockOrder stockOrder = await this.FetchAsync<StockOrder>("StockOrder/" + stockId);
            return stockOrder;
        }

        public async Task<StockOrder> CreateStockOrder(StockOrder stockOrder)
        {
            string jsonIn = JsonConvert.SerializeObject(stockOrder);
            ApiRequest apiRequest = new ApiRequest()
                                    {
                                        JsonString = jsonIn
                                    };

            string apiRequestString = JsonConvert.SerializeObject(apiRequest);
            stockOrder = await this.PostAsync<StockOrder>("StockOrder/", apiRequestString);
            return stockOrder;
        }

        //public async Task<StockOrder> UpdateStockHolder(StockOrder stockHolder)
        //{
        //    string jsonIn = JsonConvert.SerializeObject(stockHolder);
        //    ApiRequest apiRequest = new ApiRequest()
        //                            {
        //                                JsonString = jsonIn
        //                            };

        //    string apiRequestString = JsonConvert.SerializeObject(apiRequest);
        //    stockHolder = await this.UpdateAsync<StockOrder>("StockHolder/", apiRequestString);
        //    return stockHolder;
        //}
    }
}
