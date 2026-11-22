using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Stock.Models;
using DTOs = Stock.Models;
using Xunit;

namespace Stock.API.Tests.IntegrationTests
{
    public class StockOrderControllerIntegrationTests : IClassFixture<CustomWebApplicationFactory<Stock.API.Startup>>
    {
        #region Fields

        private string url = "api/stockorder/";
        private CustomWebApplicationFactory<Stock.API.Startup> _Factory;
        #endregion

        #region Constructor
        public StockOrderControllerIntegrationTests(CustomWebApplicationFactory<Stock.API.Startup> factory)
        {
            _Factory = factory;
        }
        #endregion

        #region Methods
        [Fact]
        public async Task Get_NoArgument_ReturnsStockOrderCollection()
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken(TestTokens.StockApiScope);

            //Act
            var response = await client.GetAsync(url, TestContext.Current.CancellationToken);
            var jsonString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            var stockOrders = JsonConvert.DeserializeObject<List<DTOs.StockOrder>>(jsonString);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(stockOrders);
            Assert.True(stockOrders.Count >= 2);
        }

        [Fact]
        public async Task Get_ValidStockOrderId_ReturnsStockOrder()
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken(TestTokens.StockApiScope);

            //Act
            var response = await client.GetAsync(url + 1, TestContext.Current.CancellationToken);
            var jsonString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            var stockOrder = JsonConvert.DeserializeObject<DTOs.StockOrder>(jsonString);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(stockOrder);
            Assert.Equal(1, stockOrder.StockOrderId);
            Assert.Equal(Utilities.Microsoft, stockOrder.StockId);
            Assert.Equal(OrderType.Limit, stockOrder.OrderType);
        }

        [Theory]
        [InlineData("1098", HttpStatusCode.NotFound)]
        [InlineData("not-a-number", HttpStatusCode.BadRequest)]
        public async Task Get_InvalidStockOrderId_ReturnsError(string stockOrderId, HttpStatusCode expectedStatusCode)
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken(TestTokens.StockApiScope);

            //Act
            var response = await client.GetAsync(url + stockOrderId, TestContext.Current.CancellationToken);

            //Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Fact]
        public async Task Post_ValidStockOrder_ReturnsStockOrderThatCanBeRetrieved()
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken(TestTokens.StockApiScope);
            var newStockOrder = new DTOs.StockOrder()
                                {
                                    StockId = Utilities.Google,
                                    Shares = 7,
                                    OrderType = OrderType.Stop,
                                    TransactionType = TransactionType.Sell,
                                    TimeInForce = TimeInForce.ThirtyDays,
                                    StopLimitPrice = 99.5,
                                };

            //Act
            var response = await client.PostAsync(url, GetApiRequestContent(JsonConvert.SerializeObject(newStockOrder)), TestContext.Current.CancellationToken);
            var jsonString = await response.Content.ReadAsStringAsync(TestContext.Current.CancellationToken);
            var stockOrder = JsonConvert.DeserializeObject<DTOs.StockOrder>(jsonString);

            //Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(stockOrder);
            Assert.True(stockOrder.StockOrderId > 2);
            Assert.Equal(Utilities.Google, stockOrder.StockId);
            Assert.Equal(7, stockOrder.Shares);
            Assert.Equal(OrderType.Stop, stockOrder.OrderType);
            Assert.Equal(TransactionType.Sell, stockOrder.TransactionType);
            Assert.Equal(TimeInForce.ThirtyDays, stockOrder.TimeInForce);
            Assert.Equal(99.5, stockOrder.StopLimitPrice);

            var getResponse = await client.GetAsync(url + stockOrder.StockOrderId, TestContext.Current.CancellationToken);
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        }

        [Theory]
        [InlineData("")]
        [InlineData("this is not json")]
        [InlineData("{\"shares\":5}")]
        [InlineData("{\"stockId\":\"Microsoft\",\"shares\":0}")]
        [InlineData("{\"stockId\":\"Unknown\",\"shares\":5}")]
        public async Task Post_InvalidStockOrder_ReturnsBadRequest(string stockOrderJson)
        {
            //Arrange
            var client = _Factory.CreateClient().WithAccessToken(TestTokens.StockApiScope);

            //Act
            var response = await client.PostAsync(url, GetApiRequestContent(stockOrderJson), TestContext.Current.CancellationToken);

            //Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        private static StringContent GetApiRequestContent(string stockOrderJson)
        {
            var apiRequest = new DTOs.ApiRequest()
                             {
                                 RequestId = Guid.NewGuid().ToString(),
                                 JsonString = stockOrderJson
                             };

            return new StringContent(JsonConvert.SerializeObject(apiRequest), Encoding.UTF8, "application/json");
        }
        #endregion
    }
}
