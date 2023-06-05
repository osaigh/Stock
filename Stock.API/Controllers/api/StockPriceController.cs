using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stock.API.Repository;
using ApiRequest = Stock.Models.ApiRequest;
using DAOStockPrice = Stock.API.Models.StockPrice;
using DTOStockPrice = Stock.Models.StockPrice;

namespace Stock.API.Controllers.api
{
    [Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockPriceController : ControllerBase
    {
        #region Fields

        private readonly IRepository<DAOStockPrice> _StockPriceRepository;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockPriceController(
            IRepository<DAOStockPrice> stockPriceRepository,
            IMapper mapper)
        {
            _StockPriceRepository = stockPriceRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTOStockPrice>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStockPrice>>> Get()
        {
            var daoStockPrices = await _StockPriceRepository.GetAllAsync();
            var dtoStockPrices = _Mapper.Map<List<DTOStockPrice>>(daoStockPrices);

            return Ok(dtoStockPrices);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOStockPrice), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<DTOStockPrice>> Get(int id)
        {
            var daoStockPrice = await _StockPriceRepository.GetAsync(id);

            if (daoStockPrice == null)
            {
                return new BadRequestObjectResult(string.Format("No StockPrice with id {0}", id));
            }

            var dtoStockPrice = _Mapper.Map<DTOStockPrice>(daoStockPrice);

            return Ok(dtoStockPrice);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DTOStockPrice), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStockPrice>> Post([FromBody] ApiRequest apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest.JsonString))
            {
                return new BadRequestObjectResult("JsonString is empty");
            }

            DTOStockPrice dtoStockPrice = JsonConvert.DeserializeObject<DTOStockPrice>(apiRequest.JsonString);
            if (dtoStockPrice == null || dtoStockPrice.StockId <= 0)
            {
                return new BadRequestObjectResult("StockId is invalid");
            }

            var daoStockPrice = _Mapper.Map<DAOStockPrice>(dtoStockPrice);
            daoStockPrice = await _StockPriceRepository.AddAsync(daoStockPrice);

            if (daoStockPrice == null)
            {
                return NotFound();
            }

            dtoStockPrice = _Mapper.Map<DTOStockPrice>(daoStockPrice);

            return Ok(dtoStockPrice);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(DTOStockPrice), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStockPrice>> Put([FromBody] ApiRequest apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest.JsonString))
            {
                return new BadRequestObjectResult("JsonString is empty");
            }

            DTOStockPrice dtoStockPrice = JsonConvert.DeserializeObject<DTOStockPrice>(apiRequest.JsonString);
            if (dtoStockPrice == null || dtoStockPrice.Id <= 0
                                               || dtoStockPrice.StockId <= 0)
            {
                return new BadRequestObjectResult("StockId is invalid");
            }

            var daoStockPrice = await _StockPriceRepository.GetAsync(dtoStockPrice.Id);
            if (daoStockPrice == null)
            {
                return new BadRequestObjectResult("No StockPrice");
            }

            _Mapper.Map(dtoStockPrice, daoStockPrice);

            daoStockPrice = await _StockPriceRepository.UpdateAsync(daoStockPrice);
            if (daoStockPrice == null)
            {
                return NotFound();
            }

            dtoStockPrice = _Mapper.Map<DTOStockPrice>(daoStockPrice);

            return Ok(dtoStockPrice);
        }
        #endregion
    }
}