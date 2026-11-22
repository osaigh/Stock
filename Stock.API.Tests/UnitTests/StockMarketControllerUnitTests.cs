using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Stock.API.Controllers.api;
using Stock.API.Services;
using Stock.Models;
using Xunit;

namespace Stock.API.Tests.UnitTests
{
    public class StockMarketControllerUnitTests
    {
        [Fact]
        public async Task Put_ValidRequest_UpdatesStockPrices()
        {
            //Arrange
            var mockMarketService = new Mock<IStockMarketService>();
            var stockMarketController = new StockMarketController(mockMarketService.Object);
            ApiRequest apiRequest = new ApiRequest()
                                    {
                                        RequestId = Guid.NewGuid().ToString()
                                    };

            //Act
            var result = await stockMarketController.Update(apiRequest);

            //Assert
            Assert.IsType<NoContentResult>(result);
            mockMarketService.Verify(marketService => marketService.UpdateStockPrices(), Times.Once);
        }
    }
}
