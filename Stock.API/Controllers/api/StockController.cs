using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Models;
using Stock.API.Repository;
using DAOStock = Stock.API.Models.Stock;
using DTOStock = Stock.Models.Stock;

namespace Stock.API.Controllers.api
{
    [Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : ControllerBase
    {
        #region Fields

        private readonly IRepository<DAOStock> _StockRepository;
        #endregion

        #region Constructor
        public StockController(IRepository<DAOStock> stockRepository)
        {
            _StockRepository = stockRepository;
        }
        #endregion

        #region Methods
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTOStock>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStock>>> Get()
        {
            var daoStocks = await _StockRepository.GetAllAsync();

            return Ok(daoStocks.ToDtos());
        }

        [HttpGet("{stockId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DTOStock), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStock>> Get(string stockId)
        {
            if (string.IsNullOrEmpty(stockId))
            {
                return new BadRequestObjectResult("stockId is not valid");
            }

            var daoStock = await _StockRepository.GetAsync(stockId);

            if (daoStock == null)
            {
                return NotFound($"No Stock with Id {stockId}");
            }

            return Ok(daoStock.ToDto());
        }

        #endregion
    }
}
