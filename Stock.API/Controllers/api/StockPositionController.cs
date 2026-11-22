using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stock.API.Models;
using Stock.API.Repository;
using DAOStockPosition = Stock.API.Models.StockPosition;
using DTOStockPosition = Stock.Models.StockPosition;

namespace Stock.API.Controllers.api
{
    //[Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockPositionController : ControllerBase
    {
        #region Fields
        private readonly IRepository<DAOStockPosition> _StockPositionRepository;
        #endregion

        #region Constructor
        public StockPositionController(IRepository<DAOStockPosition> stockPositionRepository)
        {
            _StockPositionRepository = stockPositionRepository;
        }
        #endregion

        #region Methods
        [HttpGet()]
        [ProducesResponseType(typeof(IEnumerable<DTOStockPosition>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStockPosition>>> Get()
        {
            var daoStockPositions = await _StockPositionRepository.GetAllAsync();

            return Ok(daoStockPositions.ToDtos());
        }

       
        #endregion
    }
}
