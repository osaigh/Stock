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
    public class StockPositionControllerUnitTests
    {
        [Fact]
        public async Task Get_NoArgument_ReturnsStockPositionCollectionWithMarketValueAndGainLoss()
        {
            //Arrange
            var daoStockPositions = Utilities.GetTestStockPositions();
            daoStockPositions[0].CurrentPrice = 100;
            daoStockPositions[1].CurrentPrice = 90;

            var mockRepo = new Mock<IRepository<DAOs.StockPosition>>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(daoStockPositions);

            var stockPositionController = new StockPositionController(mockRepo.Object);

            //Act
            var result = await stockPositionController.Get();

            //Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var stockPositions = Assert.IsAssignableFrom<IEnumerable<DTOs.StockPosition>>(okResult.Value).ToList();
            Assert.Equal(2, stockPositions.Count);

            //10 shares bought for 800 that are now worth 100 each
            Assert.Equal(Utilities.Microsoft, stockPositions[0].StockId);
            Assert.Equal(10, stockPositions[0].Shares);
            Assert.Equal(800, stockPositions[0].CostBasis);
            Assert.Equal(1000, stockPositions[0].MarketValue);
            Assert.Equal(25, stockPositions[0].GainLossPercent);

            //20 shares bought for 2000 that are now worth 90 each
            Assert.Equal(Utilities.Google, stockPositions[1].StockId);
            Assert.Equal(1800, stockPositions[1].MarketValue);
            Assert.Equal(-10, stockPositions[1].GainLossPercent);
        }

        [Fact]
        public void GainLossPercent_ZeroCostBasis_IsZero()
        {
            //A division by zero here would produce a value that cannot be serialized to JSON
            var stockPosition = new DTOs.StockPosition() { Shares = 10, CurrentPrice = 5, CostBasis = 0 };

            Assert.Equal(0, stockPosition.GainLossPercent);
        }
    }
}
