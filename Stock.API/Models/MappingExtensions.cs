using System.Collections.Generic;
using System.Linq;
using DAOS = Stock.API.Models;
using DTOs = Stock.Models;

namespace Stock.API.Models
{
    /// <summary>
    /// Maps between the database entities (DAOs) and the DTOs exposed by the API.
    /// </summary>
    public static class MappingExtensions
    {
        //Stocks
        public static DTOs.Stock ToDto(this DAOS.Stock stock)
        {
            return new DTOs.Stock()
                   {
                       StockId = stock.StockId,
                       Price = stock.Price,
                   };
        }

        public static List<DTOs.Stock> ToDtos(this IEnumerable<DAOS.Stock> stocks)
        {
            return stocks.Select(ToDto).ToList();
        }

        //StockPosition
        public static DTOs.StockPosition ToDto(this DAOS.StockPosition stockPosition)
        {
            return new DTOs.StockPosition()
                   {
                       StockPositionId = stockPosition.StockPositionId,
                       StockId = stockPosition.StockId,
                       Shares = stockPosition.Shares,
                       CurrentPrice = stockPosition.CurrentPrice,
                       CostBasis = stockPosition.CostBasis,
                   };
        }

        public static List<DTOs.StockPosition> ToDtos(this IEnumerable<DAOS.StockPosition> stockPositions)
        {
            return stockPositions.Select(ToDto).ToList();
        }

        //StockHistory
        public static DTOs.StockHistory ToDto(this DAOS.StockHistory stockHistory)
        {
            return new DTOs.StockHistory()
                   {
                       StockHistoryId = stockHistory.StockHistoryId,
                       StockId = stockHistory.StockId,
                       Date = stockHistory.Date,
                       Price = stockHistory.Price,
                   };
        }

        public static List<DTOs.StockHistory> ToDtos(this IEnumerable<DAOS.StockHistory> stockHistories)
        {
            return stockHistories.Select(ToDto).ToList();
        }

        //StockOrder
        public static DTOs.StockOrder ToDto(this DAOS.StockOrder stockOrder)
        {
            return new DTOs.StockOrder()
                   {
                       StockOrderId = stockOrder.StockOrderId,
                       StockId = stockOrder.StockId,
                       Shares = stockOrder.Shares,
                       TimeInForce = stockOrder.TimeInForce,
                       TransactionType = stockOrder.TransactionType,
                       OrderType = stockOrder.OrderType,
                       StopLimitPrice = stockOrder.StopLimitPrice,
                   };
        }

        public static List<DTOs.StockOrder> ToDtos(this IEnumerable<DAOS.StockOrder> stockOrders)
        {
            return stockOrders.Select(ToDto).ToList();
        }

        public static DAOS.StockOrder ToEntity(this DTOs.StockOrder stockOrder)
        {
            return new DAOS.StockOrder()
                   {
                       StockOrderId = stockOrder.StockOrderId,
                       StockId = stockOrder.StockId,
                       Shares = stockOrder.Shares,
                       TimeInForce = stockOrder.TimeInForce,
                       TransactionType = stockOrder.TransactionType,
                       OrderType = stockOrder.OrderType,
                       StopLimitPrice = stockOrder.StopLimitPrice,
                   };
        }
    }
}
