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
using DAOStockHolder = Stock.API.Models.StockHolder;
using DTOStockHolder = Stock.Models.StockHolder;

namespace Stock.API.Controllers.api
{
    [Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockHolderController : ControllerBase
    {
        #region Fields

        private readonly IRepository<DAOStockHolder> _StockHolderRepository;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockHolderController(
            IRepository<DAOStockHolder> stockHolderRepository,
            IMapper mapper)
        {
            _StockHolderRepository = stockHolderRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<DTOStockHolder>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStockHolder>>> Get()
        {
            var daoStockHolders = await _StockHolderRepository.GetAllAsync();

            var dtoStockHolders = _Mapper.Map<List<DTOStockHolder>>(daoStockHolders);

            return Ok(dtoStockHolders); ;
        }

        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOStockHolder), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStockHolder>> Get(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new BadRequestObjectResult("username is null");
            }

            var daoStockHolder = await _StockHolderRepository.GetAsync(username);

            if (daoStockHolder == null)
            {
                return new BadRequestObjectResult(string.Format("No Stockholder with username {0}", username));
            }

            var dtoStockHolder = _Mapper.Map<DTOStockHolder>(daoStockHolder);

            return dtoStockHolder;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOStockHolder), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStockHolder>> Post([FromBody] ApiRequest apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest.JsonString))
            {
                return new BadRequestObjectResult("JsonString is empty");
            }

            DTOStockHolder dtoStockHolder = JsonConvert.DeserializeObject<DTOStockHolder>(apiRequest.JsonString);
            
            //validate input
            if (dtoStockHolder == null || string.IsNullOrEmpty(dtoStockHolder.Username))
            {
                return new BadRequestObjectResult("Username is required");
            }

            //Add
            DAOStockHolder daoStockHolder = _Mapper.Map<DAOStockHolder>(dtoStockHolder);
            daoStockHolder.Username = dtoStockHolder.Username;
            daoStockHolder = await _StockHolderRepository.AddAsync(daoStockHolder);
            if (daoStockHolder == null)
            {
                return new BadRequestObjectResult(string.Format("No Stockholder with username {0}", dtoStockHolder.Username));
            }

            dtoStockHolder = _Mapper.Map<DTOStockHolder>(daoStockHolder);

            return Ok(dtoStockHolder);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOStockHolder), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStockHolder>> Put([FromBody] ApiRequest apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest.JsonString))
            {
                return new BadRequestObjectResult("JsonString is empty");
            }

            DTOStockHolder dtoStockHolder = JsonConvert.DeserializeObject<DTOStockHolder>(apiRequest.JsonString);
            //validate input
            if (dtoStockHolder == null || string.IsNullOrEmpty(dtoStockHolder.Username))
            {
                return new BadRequestObjectResult("Username is required");
            }

            //Update
            var daoStockHolder = await _StockHolderRepository.GetAsync(dtoStockHolder.Username);
            if (daoStockHolder == null)
            {
                return new BadRequestObjectResult(string.Format("No Stockholder with username {0}", dtoStockHolder.Username));
            }

            _Mapper.Map(dtoStockHolder, daoStockHolder);
            daoStockHolder = await _StockHolderRepository.UpdateAsync(daoStockHolder);

            if (daoStockHolder == null)
            {
                return new BadRequestObjectResult(string.Format("No Stockholder with username {0}", dtoStockHolder.Username));
            }

            dtoStockHolder = _Mapper.Map<DTOStockHolder>(daoStockHolder);

            return Ok(dtoStockHolder);
        }
        #endregion
    }
}