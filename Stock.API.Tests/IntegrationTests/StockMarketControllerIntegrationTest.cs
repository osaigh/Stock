using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DTOs = Stock.Models;
using Xunit;

namespace Stock.API.Tests.IntegrationTests
{
    public class StockMarketControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Stock.API.Startup>>
    {
        #region Fields

        private string stockMarketUrl = "api/stockmarket/";
        private string stockUrl = "api/stock/";
        private CustomWebApplicationFactory<Stock.API.Startup> _Factory;
        #endregion

        #region Constructor
        public StockMarketControllerIntegrationTest(CustomWebApplicationFactory<Stock.API.Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods
        [Fact]
        public async Task Put_ValidRequest_UpdatesStockPrices()
        {
            //Arrange
            var client = _Factory.CreateClient();
            DTOs.ApiRequest apiRequest = new DTOs.ApiRequest()
                                         {
                                             RequestId = Guid.NewGuid().ToString()
                                         };
            string apiRequestString = JsonConvert.SerializeObject(apiRequest);
            int stockId = 1;
            double oldPrice = 0.0;
            double newPrice = 0.0;

            //Act
            //Get old price
            var response = await client.GetAsync(stockUrl + stockId);
            var jsonString = await response.Content.ReadAsStringAsync();
            var stock = JsonConvert.DeserializeObject<DTOs.Stock>(jsonString);
            oldPrice = stock.Price;

            //Update the prices
            response = await client.PutAsync(stockMarketUrl, new StringContent(apiRequestString, Encoding.UTF8, "application/json"));

            //Get new price
            response = await client.GetAsync(stockUrl + stockId);
            jsonString = await response.Content.ReadAsStringAsync();
            stock = JsonConvert.DeserializeObject<DTOs.Stock>(jsonString);
            newPrice = stock.Price;

            //Assert
            Assert.NotEqual(oldPrice, newPrice);
        }
        #endregion
    }
}
