using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Models;
using Stock.API.Repository;
using DAOStockHistory = Stock.API.Models.StockHistory;
using DTOStockHistory = Stock.Models.StockHistory;

namespace Stock.API.Controllers.api
{
    [Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockHistoryController : ControllerBase
    {
        #region Fields

        private readonly IRepository<DAOStockHistory> _StockHistoryRepository;
        #endregion

        #region Constructor
        public StockHistoryController(IRepository<DAOStockHistory> stockHistoryRepository)
        {
            _StockHistoryRepository = stockHistoryRepository;
        }
        #endregion

        #region Methods
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTOStockHistory>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStockHistory>>> Get()
        {
            var daoStockHistories = await _StockHistoryRepository.GetAllAsync();

            return Ok(daoStockHistories.ToDtos());
        }

        [HttpGet("{stockId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<DTOStockHistory>), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<IEnumerable<DTOStockHistory>>> Get(string stockId)
        {
            if (string.IsNullOrEmpty(stockId))
            {
                return new BadRequestObjectResult("stockId is null");
            }

            var daoStockHistories = await _StockHistoryRepository.SearchForAsync(s => s.StockId == stockId);

            return Ok(daoStockHistories.OrderBy(s => s.Date).ToDtos());
        }

        #endregion
    }
}
