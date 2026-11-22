using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using DTOs = Stock.Models;
using Xunit;

namespace Stock.API.Tests.IntegrationTests
{
    public class StockHistoryControllerIntegrationTest : IClassFixture<CustomWebApplicationFactory<Stock.API.Startup>>
    {
        #region Fields

        private string url = "api/stockhistory/";
        private CustomWebApplicationFactory<Stock.API.Startup> _Factory;
        #endregion

        #region Constructor
        public StockHistoryControllerIntegrationTest(CustomWebApplicationFactory<Stock.API.Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods

        [Fact]
        public async Task Get_NoArgument_ReturnsStockHistoryCollection()
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken(TestTokens.StockApiScope);

            //Act
            var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
            var jsonString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            var stockHistories = JsonConvert.DeserializeObject<List<DTOs.StockHistory>>(jsonString);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(stockHistories);
            Assert.Equal(5, stockHistories.Count);
        }

        [Theory]
        [InlineData(Utilities.Microsoft, new double[] { 45, 65, 70 })]
        [InlineData(Utilities.Google, new double[] { 100, 120 })]
        public async Task Get_ValidStockId_ReturnsStockHistoryOfThatStockOrderedByDate(string stockId, double[] prices)
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken(TestTokens.StockApiScope);

            //Act
            var response = await client.GetAsync(url + stockId, TestContext.Current.CancellationToken);
            var jsonString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            var stockHistories = JsonConvert.DeserializeObject<List<DTOs.StockHistory>>(jsonString);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(stockHistories);
            Assert.All(stockHistories, s => Assert.Equal(stockId, s.StockId));
            Assert.Equal(prices, stockHistories.Select(s => s.Price));
        }

        [Fact]
        public async Task Get_UnknownStockId_ReturnsEmptyCollection()
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken(TestTokens.StockApiScope);

            //Act
            var response = await client.GetAsync(url + "Unknown", TestContext.Current.CancellationToken);
            var jsonString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            var stockHistories = JsonConvert.DeserializeObject<List<DTOs.StockHistory>>(jsonString);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(stockHistories);
            Assert.Empty(stockHistories);
        }

        #endregion
    }
}
