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
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockController(
            IRepository<DAOStock> stockRepository,
            IMapper mapper)
        {
            _StockRepository = stockRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTOStock>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStock>>> Get()
        {
            var daoStocks = await _StockRepository.GetAllAsync();
            
            var dtoStocks = _Mapper.Map<List<DTOStock>>(daoStocks);

            return Ok(dtoStocks);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOStock), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStock>> Get(int id)
        {
            if (id <= 0)
            {
                return new BadRequestObjectResult("id is null");
            }

            var daoStock = await _StockRepository.GetAsync(id);

            if (daoStock == null)
            {
                return null;
            }

            var dtoStock= _Mapper.Map<DTOStock>(daoStock);

            return Ok(dtoStock);
        }

        #endregion
    }
}