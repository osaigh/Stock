using System.Threading.Tasks;

namespace Stock.API.Services
{
    public interface IStockMarketService
    {
        Task UpdateStockPrices();
    }
}
