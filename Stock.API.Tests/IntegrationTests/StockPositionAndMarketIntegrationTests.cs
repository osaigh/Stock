using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DTOs = Stock.Models;
using Xunit;

namespace Stock.API.Tests.IntegrationTests
{
    public class StockPositionAndMarketIntegrationTests : IClassFixture<CustomWebApplicationFactory<Stock.API.Startup>>
    {
        #region Fields

        private string stockPositionUrl = "api/stockposition/";
        private string stockMarketUrl = "api/stockmarket/";
        private CustomWebApplicationFactory<Stock.API.Startup> _Factory;
        #endregion

        #region Constructor
        public StockPositionAndMarketIntegrationTests(CustomWebApplicationFactory<Stock.API.Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods
        [Fact]
        public async Task Get_NoArgument_ReturnsStockPositionCollection()
        {
            //Arrange
            var client = _Factory.CreateClient();

            //Act
            var stockPositions = await GetStockPositions(client);

            //Assert
            Assert.Equal(2, stockPositions.Count);
            var microsoft = stockPositions.Single(s => s.StockId == Utilities.Microsoft);
            Assert.Equal(10, microsoft.Shares);
            Assert.Equal(800, microsoft.CostBasis);
            Assert.True(microsoft.CurrentPrice > 0);
            Assert.Equal(microsoft.Shares * microsoft.CurrentPrice, microsoft.MarketValue, 2);
        }

        [Fact]
        public async Task Put_StockMarket_UpdatesTheCurrentPriceOfThePositions()
        {
            //Arrange
            var client = _Factory.CreateClient();
            var before = await GetStockPositions(client);
            var apiRequest = new StringContent(JsonConvert.SerializeObject(new DTOs.ApiRequest()), Encoding.UTF8, "application/json");

            //Act
            var response = await client.PutAsync(stockMarketUrl, apiRequest, TestContext.Current.CancellationToken);
            var after = await GetStockPositions(client);

            //Assert
            Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
            Assert.NotEqual(before.Select(s => s.CurrentPrice), after.Select(s => s.CurrentPrice));
            Assert.All(after, s => Assert.True(s.CurrentPrice > 0));
        }

        private async Task<List<DTOs.StockPosition>> GetStockPositions(HttpClient client)
        {
            var response = await client.GetAsync(stockPositionUrl, TestContext.Current.CancellationToken);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var jsonString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            var stockPositions = JsonConvert.DeserializeObject<List<DTOs.StockPosition>>(jsonString);
            Assert.NotNull(stockPositions);

            return stockPositions;
        }
        #endregion
    }
}
