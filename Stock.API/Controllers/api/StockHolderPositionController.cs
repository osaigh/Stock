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
using DAOStockHolderPosition = Stock.API.Models.StockHolderPosition;
using DTOStockHolderPosition = Stock.Models.StockHolderPosition;

namespace Stock.API.Controllers.api
{
    [Authorize("StockAPIPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class StockHolderPositionController : ControllerBase
    {
        #region Fields
        private readonly IRepository<DAOStockHolderPosition> _StockHolderPositionRepository;
        private readonly IMapper _Mapper;
        #endregion

        #region Constructor
        public StockHolderPositionController(
            IRepository<DAOStockHolderPosition> stockHolderPositionRepository,
            IMapper mapper)
        {
            _StockHolderPositionRepository = stockHolderPositionRepository;
            _Mapper = mapper;
        }
        #endregion

        #region Methods
        [HttpGet("{username}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(IEnumerable<DTOStockHolderPosition>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<DTOStockHolderPosition>>> Get(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return new BadRequestObjectResult("username is required");
            }

            var daoStockHolderPositions = await _StockHolderPositionRepository.SearchForAsync(s => s.StockHolderId.ToLower() == username.ToLower());

            var dtoStockHolderPositions = _Mapper.Map<List<DTOStockHolderPosition>>(daoStockHolderPositions.ToList());

            return Ok(dtoStockHolderPositions);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(DTOStockHolderPosition), (int)(HttpStatusCode.OK))]
        public async Task<ActionResult<DTOStockHolderPosition>> Post([FromBody] ApiRequest apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest.JsonString))
            {
                return new BadRequestObjectResult("JsonString is empty");
            }

            DTOStockHolderPosition dtoStockHolderPosition = JsonConvert.DeserializeObject<DTOStockHolderPosition>(apiRequest.JsonString);
            if (dtoStockHolderPosition == null || string.IsNullOrEmpty(dtoStockHolderPosition.StockHolderId)
                                                || dtoStockHolderPosition.StockId <= 0)
            {
                return new BadRequestObjectResult("StockId is invalid");
            }

            var daoStockHolderPosition = _Mapper.Map<DAOStockHolderPosition>(dtoStockHolderPosition);
            daoStockHolderPosition.StockId = dtoStockHolderPosition.StockId;
            daoStockHolderPosition.StockHolderId = dtoStockHolderPosition.StockHolderId;

            daoStockHolderPosition = await _StockHolderPositionRepository.AddAsync(daoStockHolderPosition);

            if (daoStockHolderPosition == null)
            {
                return new BadRequestObjectResult(string.Format("No StockHolderPosition with StockId {0}", dtoStockHolderPosition.StockId));
            }

            dtoStockHolderPosition = _Mapper.Map<DTOStockHolderPosition>(daoStockHolderPosition);

            return Ok(dtoStockHolderPosition);
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult> Delete(int id)
        {
            var daoStockHolderPosition = await _StockHolderPositionRepository.GetAsync(id);
            if (daoStockHolderPosition == null)
            {
                return NotFound();
            }

            await _StockHolderPositionRepository.DeleteAsync(daoStockHolderPosition);
            return NoContent();
        }
        #endregion
    }
}