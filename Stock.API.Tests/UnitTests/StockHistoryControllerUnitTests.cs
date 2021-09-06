using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Moq;
using Newtonsoft.Json;
using Stock.API.Controllers.api;
using Stock.API.Repository;
using DAOs = Stock.API.Models;
using DTOs = Stock.Models;
using Xunit;
using System.Threading.Tasks;

namespace Stock.API.Tests.UnitTests
{
    public class StockHistoryControllerUnitTests
    {
        #region Fields

        #endregion

        #region Constructor
        public StockHistoryControllerUnitTests()
        {

        }
        #endregion

        #region Methods

        [Fact]
        public async Task Get_NoArgument_ReturnsStockHistoryCollection()
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //StockHolder Repo
            var mockRepo = new Mock<IRepository<DAOs.StockHistory>>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(Utilities.GetTestStockHistories());

            //Controler
            var stockHistoryController = new StockHistoryController(mockRepo.Object, mapper);

            //Act
            var stockHistories = await stockHistoryController.Get();

            //Assert
            Assert.NotNull(stockHistories);
            Assert.Equal(3, stockHistories.Count());
        }

        [Theory]
        [InlineData(1)]
        public async Task Get_ValidStockId_ReturnsStockHistoryCollection(int stockId)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //StockHolder Repo
            var mockRepo = new Mock<IRepository<DAOs.StockHistory>>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(Utilities.GetTestStockHistories());

            var stockHistoryController = new StockHistoryController(mockRepo.Object, mapper);

            //Act
            var stockHistories = await stockHistoryController.Get(stockId);

            //Assert
            Assert.NotNull(stockHistories);
            Assert.Equal(3, stockHistories.Count());
        }

        [Theory]
        [InlineData(1098)]
        public async Task Get_InvalidStockId_ReturnsEmptyCollection(int stockId)
        {
            //Arrange

            //AutoMapper
            var mapper = GetMapper();

            //StockHolder Repo
            var mockRepo = new Mock<IRepository<DAOs.StockHistory>>();
            mockRepo.Setup(repo => repo.GetAllAsync()).ReturnsAsync(Utilities.GetTestStockHistories());

            var stockHistoryController = new StockHistoryController(mockRepo.Object, mapper);

            //Act
            var stockHistories = await stockHistoryController.Get(stockId);

            //Assert
            Assert.NotNull(stockHistories);
            Assert.Empty(stockHistories);
        }

        #endregion

        #region Helpers
        private IMapper GetMapper()
        {
            //AutoMapper
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map<DTOs.StockHistory>(It.IsAny<DAOs.StockHistory>())).Returns((DAOs.StockHistory input) =>
            {
                var dtoStockHistory = new DTOs.StockHistory
                {
                                          Id = input.Id,
                                          StockId = input.StockId,
                                          Date = input.Date,
                                          Price = input.Price
                                      };

                return dtoStockHistory;
            });

            mockMapper.Setup(mapper => mapper.Map<List<DTOs.StockHistory>>(It.IsAny<List<DAOs.StockHistory>>())).Returns((List<DAOs.StockHistory> inputs) =>
            {
                return inputs.Select(input => new DTOs.StockHistory()
                {
                    Id = input.Id,
                    StockId = input.StockId,
                    Date = input.Date,
                    Price = input.Price
                })
                                        .ToList();
            });

            return mockMapper.Object;
        }

        #endregion
    }
}
