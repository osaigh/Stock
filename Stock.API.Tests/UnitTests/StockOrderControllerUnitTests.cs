using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json;
using Stock.API.Controllers.api;
using Stock.API.Repository;
using Stock.Models;
using Xunit;
using DAOs = Stock.API.Models;
using DTOs = Stock.Models;

namespace Stock.API.Tests.UnitTests
{
    public class StockOrderControllerUnitTests
    {
        [Fact]
        public async Task Get_NoArgument_ReturnsStockOrderCollection()
        {
            //Arrange
            var stockOrderController = new StockOrderController(GetStockOrderRepository(Utilities.GetTestStockOrders()));

            //Act
            var result = await stockOrderController.Get();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var stockOrders = Assert.IsAssignableFrom<IEnumerable<DTOs.StockOrder>>(okResult.Value).ToList();
            Assert.Equal(2, stockOrders.Count);
            Assert.Equal(OrderType.Limit, stockOrders[0].OrderType);
            Assert.Equal(TransactionType.Sell, stockOrders[1].TransactionType);
        }

        [Fact]
        public async Task Get_ValidStockOrderId_ReturnsStockOrder()
        {
            //Arrange
            var stockOrderController = new StockOrderController(GetStockOrderRepository(Utilities.GetTestStockOrders()));

            //Act
            var result = await stockOrderController.Get("2");

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var stockOrder = Assert.IsType<DTOs.StockOrder>(okResult.Value);
            Assert.Equal(2, stockOrder.StockOrderId);
            Assert.Equal(Utilities.Google, stockOrder.StockId);
        }

        [Fact]
        public async Task Get_UnknownStockOrderId_ReturnsNotFound()
        {
            //Arrange
            var stockOrderController = new StockOrderController(GetStockOrderRepository(Utilities.GetTestStockOrders()));

            //Act
            var result = await stockOrderController.Get("1098");

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("not-a-number")]
        public async Task Get_InvalidStockOrderId_ReturnsBadRequest(string stockOrderId)
        {
            //Arrange
            var stockOrderController = new StockOrderController(GetStockOrderRepository(Utilities.GetTestStockOrders()));

            //Act
            var result = await stockOrderController.Get(stockOrderId);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task Post_ValidStockOrder_AddsAndReturnsStockOrder()
        {
            //Arrange
            var stockOrders = Utilities.GetTestStockOrders();
            var stockOrderController = new StockOrderController(GetStockOrderRepository(stockOrders));
            var newStockOrder = new DTOs.StockOrder()
                                {
                                    StockId = Utilities.Microsoft,
                                    Shares = 12,
                                    OrderType = OrderType.Stop,
                                    TransactionType = TransactionType.Sell,
                                    TimeInForce = TimeInForce.ThirtyDays,
                                    StopLimitPrice = 90.5,
                                };

            //Act
            var result = await stockOrderController.Post(GetApiRequest(JsonConvert.SerializeObject(newStockOrder)));

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var stockOrder = Assert.IsType<DTOs.StockOrder>(okResult.Value);
            Assert.Equal(3, stockOrder.StockOrderId);
            Assert.Equal(Utilities.Microsoft, stockOrder.StockId);
            Assert.Equal(12, stockOrder.Shares);
            Assert.Equal(OrderType.Stop, stockOrder.OrderType);
            Assert.Equal(TransactionType.Sell, stockOrder.TransactionType);
            Assert.Equal(TimeInForce.ThirtyDays, stockOrder.TimeInForce);
            Assert.Equal(90.5, stockOrder.StopLimitPrice);
            Assert.Equal(3, stockOrders.Count);
        }

        [Fact]
        public async Task Post_StockOrderAsSentByTheWebClient_AddsStockOrder()
        {
            //Arrange
            //The web clients send the enums by name and may send the numbers as strings
            string jsonString = "{\"stockId\":\"Google\",\"shares\":\"4\",\"timeInForce\":\"EndOfDay\",\"transactionType\":\"buy\",\"orderType\":\"Limit\",\"stopLimitPrice\":\"101.25\"}";
            var stockOrderController = new StockOrderController(GetStockOrderRepository(Utilities.GetTestStockOrders()));

            //Act
            var result = await stockOrderController.Post(GetApiRequest(jsonString));

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var stockOrder = Assert.IsType<DTOs.StockOrder>(okResult.Value);
            Assert.Equal(Utilities.Google, stockOrder.StockId);
            Assert.Equal(4, stockOrder.Shares);
            Assert.Equal(TransactionType.Buy, stockOrder.TransactionType);
            Assert.Equal(OrderType.Limit, stockOrder.OrderType);
            Assert.Equal(101.25, stockOrder.StopLimitPrice);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("this is not json")]
        [InlineData("{\"shares\":5}")]
        [InlineData("{\"stockId\":\"Microsoft\",\"shares\":0}")]
        [InlineData("{\"stockId\":\"Microsoft\",\"shares\":-3}")]
        [InlineData("{\"stockId\":\"Unknown\",\"shares\":5}")]
        public async Task Post_InvalidStockOrder_ReturnsBadRequest(string jsonString)
        {
            //Arrange
            var stockOrders = Utilities.GetTestStockOrders();
            var stockOrderController = new StockOrderController(GetStockOrderRepository(stockOrders));

            //Act
            var result = await stockOrderController.Post(GetApiRequest(jsonString));

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
            Assert.Equal(2, stockOrders.Count);
        }

        private static ApiRequest GetApiRequest(string jsonString)
        {
            return new ApiRequest()
                   {
                       RequestId = Guid.NewGuid().ToString(),
                       JsonString = jsonString
                   };
        }

        /// <summary>
        /// A repository that behaves like StockOrderRepository: orders for unknown stocks are not added
        /// </summary>
        private static IRepository<DAOs.StockOrder> GetStockOrderRepository(List<DAOs.StockOrder> stockOrders)
        {
            var stocks = Utilities.GetTestStocks();
            var mockRepo = new Mock<IRepository<DAOs.StockOrder>>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(stockOrders);
            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<object>()))
                    .ReturnsAsync((object id) => stockOrders.FirstOrDefault(s => s.StockOrderId == (int)id));
            mockRepo.Setup(repo => repo.AddAsync(It.IsAny<DAOs.StockOrder>()))
                    .ReturnsAsync((DAOs.StockOrder stockOrder) =>
                                  {
                                      if (stocks.All(s => s.StockId != stockOrder.StockId))
                                      {
                                          return null;
                                      }

                                      stockOrder.StockOrderId = stockOrders.Max(s => s.StockOrderId) + 1;
                                      stockOrders.Add(stockOrder);
                                      return stockOrder;
                                  });

            return mockRepo.Object;
        }
    }
}
