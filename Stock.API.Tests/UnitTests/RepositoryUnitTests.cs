using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.API.Data;
using Stock.API.Repository;
using Stock.API.Services;
using Stock.Models;
using Xunit;
using DAOs = Stock.API.Models;

namespace Stock.API.Tests.UnitTests
{
    /// <summary>
    /// Tests for the repositories and the market service, running against an in-memory database
    /// </summary>
    public class RepositoryUnitTests
    {
        [Fact]
        public async Task StockOrderRepository_AddAsync_AssignsTheNextStockOrderId()
        {
            //Arrange
            using var db = GetDbContext();
            var repository = new StockOrderRepository(db);

            //Act
            var stockOrder = await repository.AddAsync(new DAOs.StockOrder() { StockId = Utilities.Microsoft, Shares = 1, OrderType = OrderType.Market });

            //Assert
            Assert.NotNull(stockOrder);
            Assert.Equal(3, stockOrder.StockOrderId);
            Assert.Equal(3, await db.StockOrders.CountAsync(TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task StockOrderRepository_AddAsync_UnknownStock_ReturnsNull()
        {
            //Arrange
            using var db = GetDbContext();
            var repository = new StockOrderRepository(db);

            //Act
            var stockOrder = await repository.AddAsync(new DAOs.StockOrder() { StockId = "Unknown", Shares = 1 });

            //Assert
            Assert.Null(stockOrder);
            Assert.Equal(2, await db.StockOrders.CountAsync(TestContext.Current.CancellationToken));
        }

        [Fact]
        public async Task StockOrderRepository_UpdateAsync_UpdatesTheStoredStockOrder()
        {
            //Arrange
            using var db = GetDbContext();
            var repository = new StockOrderRepository(db);

            //Act
            var stockOrder = await repository.UpdateAsync(new DAOs.StockOrder() { StockOrderId = 1, StockId = Utilities.Microsoft, Shares = 42, OrderType = OrderType.Stop });

            //Assert
            Assert.NotNull(stockOrder);
            var stored = await db.StockOrders.SingleAsync(s => s.StockOrderId == 1, TestContext.Current.CancellationToken);
            Assert.Equal(42, stored.Shares);
            Assert.Equal(OrderType.Stop, stored.OrderType);
        }

        [Fact]
        public async Task StockPositionRepository_GetAllAsync_SetsCurrentPriceFromTheStock()
        {
            //Arrange
            using var db = GetDbContext();
            var repository = new StockPositionRepository(db);

            //Act
            var stockPositions = (await repository.GetAllAsync()).ToList();

            //Assert
            Assert.Equal(2, stockPositions.Count);
            Assert.Equal(85, stockPositions.Single(s => s.StockId == Utilities.Microsoft).CurrentPrice);
            Assert.Equal(105, stockPositions.Single(s => s.StockId == Utilities.Google).CurrentPrice);
        }

        [Fact]
        public async Task StockPositionRepository_SearchForAsync_SetsCurrentPriceFromTheStock()
        {
            //Arrange
            using var db = GetDbContext();
            var repository = new StockPositionRepository(db);

            //Act
            var stockPositions = (await repository.SearchForAsync(s => s.StockId == Utilities.Google)).ToList();

            //Assert
            var stockPosition = Assert.Single(stockPositions);
            Assert.Equal(105, stockPosition.CurrentPrice);
        }

        [Fact]
        public async Task StockPositionRepository_GetAsync_UnknownId_ReturnsNull()
        {
            //Arrange
            using var db = GetDbContext();
            var repository = new StockPositionRepository(db);

            //Act
            var stockPosition = await repository.GetAsync(1098);

            //Assert
            Assert.Null(stockPosition);
        }

        [Fact]
        public async Task StockHistoryRepository_SearchForAsync_ReturnsTheHistoryOfTheStock()
        {
            //Arrange
            using var db = GetDbContext();
            var repository = new StockHistoryRepository(db);

            //Act
            var stockHistories = (await repository.SearchForAsync(s => s.StockId == Utilities.Microsoft)).ToList();

            //Assert
            Assert.Equal(3, stockHistories.Count);
        }

        [Fact]
        public async Task StockMarketService_UpdateStockPrices_KeepsEveryPricePositiveAndWithinFiveOfTheOldPrice()
        {
            //Arrange
            using var db = GetDbContext();
            db.Stocks.Add(new DAOs.Stock() { StockId = "Penny", Price = 0.5 });
            await db.SaveChangesAsync(TestContext.Current.CancellationToken);
            var oldPrices = await db.Stocks.ToDictionaryAsync(s => s.StockId, s => s.Price, TestContext.Current.CancellationToken);
            var stockMarketService = new StockMarketService(db);

            //Act
            await stockMarketService.UpdateStockPrices();

            //Assert
            foreach (var stock in await db.Stocks.ToListAsync(TestContext.Current.CancellationToken))
            {
                Assert.True(stock.Price > 0, $"{stock.StockId} has price {stock.Price}");
                if (stock.StockId != "Penny")
                {
                    Assert.InRange(stock.Price, oldPrices[stock.StockId] - 5, oldPrices[stock.StockId] + 5);
                }
            }
        }

        private static StockDbContext GetDbContext()
        {
            var options = new DbContextOptionsBuilder<StockDbContext>()
                          .UseInMemoryDatabase("RepositoryUnitTests-" + Guid.NewGuid())
                          .Options;

            var db = new StockDbContext(options);
            Utilities.InitializeDbForTests(db);

            //Start every test without tracked entities, like a new request does
            db.ChangeTracker.Clear();

            return db;
        }
    }
}
