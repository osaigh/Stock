using System.Threading.Tasks;
using DAOStockHolderPosition = Stock.API.Models.StockHolderPosition;

namespace Stock.API.Services
{
    public interface IStockMarketService
    {
        Task UpdateStockPrices();
    }
}
