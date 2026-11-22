using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Services;
using Stock.Models;

namespace Stock.API.Controllers.api
{
    //[Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockMarketController : ControllerBase
    {
        #region Fields

        private readonly IStockMarketService _StockMarketService;

        #endregion

        #region Constructor
        public StockMarketController(IStockMarketService stockMarketService)
        {
            _StockMarketService = stockMarketService;
        }
        #endregion

        #region Methods

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Update([FromBody] ApiRequest apiRequest)
        {
            await _StockMarketService.UpdateStockPrices();
            return NoContent();
        }
        #endregion
    }
}
