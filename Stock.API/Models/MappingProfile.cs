using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DAOS = Stock.API.Models;
using DTOs = Stock.Models;

namespace Stock.API.Models
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //Stocks
            CreateMap<DAOS.Stock, DTOs.Stock>();
            CreateMap<DTOs.Stock, DAOS.Stock>()
                .ForMember(dest => dest.StockId, opt => opt.Ignore());

            //StockPosition
            CreateMap<DAOS.StockPosition, DTOs.StockPosition>()
                .ForMember(dest => dest.MarketValue, opt => opt.Ignore())
                .ForMember(dest => dest.GainLossPercent, opt => opt.Ignore());
            CreateMap<DTOs.StockPosition, DAOS.StockPosition>()
                .ForMember(dest => dest.Stock, opt => opt.Ignore());


            //StockHistory
            CreateMap<DAOS.StockHistory, DTOs.StockHistory>();
            CreateMap<DTOs.StockHistory, StockHistory>()
                .ForMember(dest => dest.StockHistoryId, opt => opt.Ignore())
                .ForMember(dest => dest.StockId, opt => opt.Ignore())
                .ForMember(dest => dest.Stock, opt => opt.Ignore());

            //StockOrder
            CreateMap<DAOS.StockOrder, DTOs.StockOrder>();
            CreateMap<DTOs.StockOrder, DAOS.StockOrder>();

        }
    }
}
