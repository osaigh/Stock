using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DTOs = Stock.Models;
using Xunit;

namespace Stock.API.Tests.IntegrationTests
{
    public class StockControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Stock.API.Startup>>
    {
        #region Fields

        private string url = "api/stock/";
        private CustomWebApplicationFactory<Stock.API.Startup> _Factory;
        #endregion

        #region Constructor
        public StockControllerIntegrationTest(CustomWebApplicationFactory<Stock.API.Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods

        [Fact]
        public async Task Get_NoArgument_ReturnsStockCollection()
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken(TestTokens.StockApiScope);

            //Act
            var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
            var jsonString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            var stocks = JsonConvert.DeserializeObject<List<DTOs.Stock>>(jsonString);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(stocks);
            Assert.Equal(2, stocks.Count);
        }

        [Theory]
        [InlineData(Utilities.Microsoft, 85)]
        [InlineData(Utilities.Google, 105)]
        public async Task Get_ValidStockId_ReturnsStock(string stockId, double price)
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken(TestTokens.StockApiScope);

            //Act
            var response = await client.GetAsync(url + stockId, TestContext.Current.CancellationToken);
            var jsonString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            var stock = JsonConvert.DeserializeObject<DTOs.Stock>(jsonString);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(stock);
            Assert.Equal(stockId, stock.StockId);
            Assert.Equal(price, stock.Price);
        }

        [Fact]
        public async Task Get_UnknownStockId_ReturnsNotFound()
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken(TestTokens.StockApiScope);

            //Act
            var response = await client.GetAsync(url + "Unknown", TestContext.Current.CancellationToken);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        #endregion
    }
}
