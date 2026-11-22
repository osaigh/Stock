using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
    public class StockHistoryControllerUnitTests
    {
        [Fact]
        public async Task Get_NoArgument_ReturnsStockHistoryCollection()
        {
            //Arrange
            var stockHistoryController = new StockHistoryController(GetStockHistoryRepository());

            //Act
            var result = await stockHistoryController.Get();

            //Assert
            var stockHistories = GetStockHistories(result);
            Assert.Equal(5, stockHistories.Count);
        }

        [Fact]
        public async Task Get_ValidStockId_ReturnsStockHistoryOfThatStockOrderedByDate()
        {
            //Arrange
            var stockHistoryController = new StockHistoryController(GetStockHistoryRepository());

            //Act
            var result = await stockHistoryController.Get(Utilities.Microsoft);

            //Assert
            var stockHistories = GetStockHistories(result);
            Assert.Equal(3, stockHistories.Count);
            Assert.All(stockHistories, s => Assert.Equal(Utilities.Microsoft, s.StockId));
            Assert.Equal(new double[] { 45, 65, 70 }, stockHistories.Select(s => s.Price));
        }

        [Fact]
        public async Task Get_UnknownStockId_ReturnsEmptyCollection()
        {
            //Arrange
            var stockHistoryController = new StockHistoryController(GetStockHistoryRepository());

            //Act
            var result = await stockHistoryController.Get("Unknown");

            //Assert
            Assert.Empty(GetStockHistories(result));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        public async Task Get_EmptyStockId_ReturnsBadRequest(string stockId)
        {
            //Arrange
            var stockHistoryController = new StockHistoryController(GetStockHistoryRepository());

            //Act
            var result = await stockHistoryController.Get(stockId);

            //Assert
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        private static List<DTOs.StockHistory> GetStockHistories(ActionResult<IEnumerable<DTOs.StockHistory>> result)
        {
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            return Assert.IsAssignableFrom<IEnumerable<DTOs.StockHistory>>(okResult.Value).ToList();
        }

        private static IRepository<DAOs.StockHistory> GetStockHistoryRepository()
        {
            var stockHistories = Utilities.GetTestStockHistories();
            var mockRepo = new Mock<IRepository<DAOs.StockHistory>>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(stockHistories);
            mockRepo.Setup(repo => repo.SearchForAsync(It.IsAny<Expression<Func<DAOs.StockHistory, bool>>>()))
                    .ReturnsAsync((Expression<Func<DAOs.StockHistory, bool>> predicate) => stockHistories.AsQueryable().Where(predicate).ToList());

            return mockRepo.Object;
        }
    }
}
