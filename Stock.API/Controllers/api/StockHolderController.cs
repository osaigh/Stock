using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IEnumerable<DTOStockHolder>> Get()
        {
            var daoStockHolders = await _StockHolderRepository.GetAllAsync();

            var dtoStockHolders = _Mapper.Map<List<DTOStockHolder>>(daoStockHolders);

            return dtoStockHolders;
        }

        [HttpGet("{username}")]
        public async Task<DTOStockHolder> Get(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return null;
            }

            var daoStockHolder = await _StockHolderRepository.GetAsync(username);

            if (daoStockHolder == null)
            {
                return null;
            }

            var dtoStockHolder = _Mapper.Map<DTOStockHolder>(daoStockHolder);

            return dtoStockHolder;
        }

        [HttpPost]
        public async Task<DTOStockHolder> Post([FromBody] ApiRequest apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest.JsonString))
            {
                return null;
            }

            DTOStockHolder dtoStockHolder = JsonConvert.DeserializeObject<DTOStockHolder>(apiRequest.JsonString);
            
            //validate input
            if (dtoStockHolder == null || string.IsNullOrEmpty(dtoStockHolder.Username))
            {
                return null;
            }

            //Add
            DAOStockHolder daoStockHolder = _Mapper.Map<DAOStockHolder>(dtoStockHolder);
            daoStockHolder.Username = dtoStockHolder.Username;
            daoStockHolder = await _StockHolderRepository.AddAsync(daoStockHolder);
            if (daoStockHolder == null)
            {
                return null;
            }

            dtoStockHolder = _Mapper.Map<DTOStockHolder>(daoStockHolder);

            return dtoStockHolder;
        }

        [HttpPut]
        public async Task<DTOStockHolder> Put([FromBody] ApiRequest apiRequest)
        {
            if (string.IsNullOrEmpty(apiRequest.JsonString))
            {
                return null;
            }

            DTOStockHolder dtoStockHolder = JsonConvert.DeserializeObject<DTOStockHolder>(apiRequest.JsonString);
            //validate input
            if (dtoStockHolder == null || string.IsNullOrEmpty(dtoStockHolder.Username))
            {
                return null;
            }

            //Update
            var daoStockHolder = await _StockHolderRepository.GetAsync(dtoStockHolder.Username);
            if (daoStockHolder == null)
            {
                return null;
            }

            _Mapper.Map(dtoStockHolder, daoStockHolder);
            daoStockHolder = await _StockHolderRepository.UpdateAsync(daoStockHolder);

            if (daoStockHolder == null)
            {
                return null;
            }

            dtoStockHolder = _Mapper.Map<DTOStockHolder>(daoStockHolder);

            return dtoStockHolder;
        }
        #endregion
    }
}