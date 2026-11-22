using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stock.API.Models;
using Stock.API.Repository;
using ApiRequest = Stock.Models.ApiRequest;
using DAOStockOrder = Stock.API.Models.StockOrder;
using DTOStockOrder = Stock.Models.StockOrder;

namespace Stock.API.Controllers.api
{
    [Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockOrderController : ControllerBase
    {
        #region Fields

        private readonly IRepository<DAOStockOrder> _StockOrderRepository;
        #endregion

        #region Constructor
        public StockOrderController(IRepository<DAOStockOrder> stockOrderRepository)
        {
            _StockOrderRepository = stockOrderRepository;
        }
        #endregion

        #region Methods
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTOStockOrder>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStockOrder>>> Get()
        {
            var daoStockOrders = await _StockOrderRepository.GetAllAsync();

            return Ok(daoStockOrders.ToDtos());
        }

        [HttpGet("{stockOrderId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DTOStockOrder), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStockOrder>> Get(string stockOrderId)
        {
            if (!int.TryParse(stockOrderId, out int id))
            {
                return new BadRequestObjectResult("stockOrderId is not valid");
            }

            var daoStockOrder = await _StockOrderRepository.GetAsync(id);

            if (daoStockOrder == null)
            {
                return NotFound($"No Stock Order with Id {id}");
            }

            return Ok(daoStockOrder.ToDto());
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOStockOrder), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStockOrder>> Post([FromBody] ApiRequest apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest.JsonString))
            {
                return new BadRequestObjectResult("JsonString is empty");
            }

            DTOStockOrder dtoStockOrder;
            try
            {
                dtoStockOrder = JsonConvert.DeserializeObject<DTOStockOrder>(apiRequest.JsonString);
            }
            catch (JsonException)
            {
                return new BadRequestObjectResult("JsonString is not a valid Stock Order");
            }

            //validate input
            if (dtoStockOrder == null || string.IsNullOrEmpty(dtoStockOrder.StockId))
            {
                return new BadRequestObjectResult("StockId is required");
            }

            if (dtoStockOrder.Shares <= 0)
            {
                return new BadRequestObjectResult("Shares must be greater than zero");
            }

            //Add
            DAOStockOrder daoStockOrder = await _StockOrderRepository.AddAsync(dtoStockOrder.ToEntity());
            if (daoStockOrder == null)
            {
                return new BadRequestObjectResult($"No Stock with Id {dtoStockOrder.StockId}");
            }

            return Ok(daoStockOrder.ToDto());
        }

       
        #endregion
    }
}
