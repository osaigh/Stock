using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stock.API.Controllers.api;
using Stock.API.Repository;
using Xunit;
using DAOs = Stock.API.Models;
using DTOs = Stock.Models;

namespace Stock.API.Tests.UnitTests
{
    public class StockControllerUnitTests
    {
        [Fact]
        public async Task Get_NoArgument_ReturnsStockCollection()
        {
            //Arrange
            var stockController = new StockController(GetStockRepository());

            //Act
            var result = await stockController.Get();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var stocks = Assert.IsAssignableFrom<IEnumerable<DTOs.Stock>>(okResult.Value);
            Assert.Equal(new[] { Utilities.Microsoft, Utilities.Google }, stocks.Select(s => s.StockId));
        }

        [Theory]
        [InlineData(Utilities.Microsoft, 85)]
        [InlineData(Utilities.Google, 105)]
        public async Task Get_ValidStockId_ReturnsStock(string stockId, double price)
        {
            //Arrange
            var stockController = new StockController(GetStockRepository());

            //Act
            var result = await stockController.Get(stockId);

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var stock = Assert.IsType<DTOs.Stock>(okResult.Value);
            Assert.Equal(stockId, stock.StockId);
            Assert.Equal(price, stock.Price);
        }

        [Fact]
        public async Task Get_UnknownStockId_ReturnsNotFound()
        {
            //Arrange
            var stockController = new StockController(GetStockRepository());

            //Act
            var result = await stockController.Get("Unknown");

            //Assert
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Get_EmptyStockId_ReturnsBadRequest(string stockId)
        {
            //Arrange
            var stockController = new StockController(GetStockRepository());

            //Act
            var result = await stockController.Get(stockId);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        private static IRepository<DAOs.Stock> GetStockRepository()
        {
            var stocks = Utilities.GetTestStocks();
            var mockRepo = new Mock<IRepository<DAOs.Stock>>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(stocks);
            mockRepo.Setup(repo => repo.GetAsync(It.IsAny<object>()))
                    .ReturnsAsync((object id) => stocks.FirstOrDefault(s => s.StockId == id.ToString()));

            return mockRepo.Object;
        }
    }
}
