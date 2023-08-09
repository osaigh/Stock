using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Stock.API.Data;
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
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockOrderController(
            IRepository<DAOStockOrder> stockOrderRepository,
            IMapper mapper)
        {
            _StockOrderRepository = stockOrderRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTOStockOrder>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStockOrder>>> Get()
        {
            var daoStockOrders = await _StockOrderRepository.GetAllAsync();

            var dtoStockOrders = _Mapper.Map<List<DTOStockOrder>>(daoStockOrders);

            return Ok(dtoStockOrders); ;
        }

        [HttpGet("{stockId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOStockOrder), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStockOrder>> Get(string stockId)
        {
            if (string.IsNullOrEmpty(stockId))
            {
                return new BadRequestObjectResult("stockId is null");
            }

            var daoStockOrder = await _StockOrderRepository.GetAsync(stockId);

            if (daoStockOrder == null)
            {
                return new BadRequestObjectResult(string.Format("No Stock with Id ", stockId));
            }

            var dtoStockOrder = _Mapper.Map<DTOStockOrder>(daoStockOrder);

            return dtoStockOrder;
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

            DTOStockOrder dtoStockOrder = JsonConvert.DeserializeObject<DTOStockOrder>(apiRequest.JsonString);
            
            //validate input
            if (dtoStockOrder == null || string.IsNullOrEmpty(dtoStockOrder.StockId))
            {
                return new BadRequestObjectResult("StockId is required");
            }

            //Add
            DAOStockOrder daoStockOrder = _Mapper.Map<DAOStockOrder>(dtoStockOrder);
            daoStockOrder.StockId = dtoStockOrder.StockId;
            daoStockOrder = await _StockOrderRepository.AddAsync(daoStockOrder);
            if (daoStockOrder == null)
            {
                return new BadRequestObjectResult(string.Format("Something went wrong when placing the order."));
            }

            dtoStockOrder = _Mapper.Map<DTOStockOrder>(daoStockOrder);

            return Ok(dtoStockOrder);
        }

       
        #endregion
    }
}