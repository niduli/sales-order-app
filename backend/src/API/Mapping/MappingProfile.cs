using API.Models;
using AutoMapper;
using Domain.Entities;

namespace API.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Item, ItemDto>().ReverseMap();
            CreateMap<SalesOrderLine, OrderLineDto>().ReverseMap();
            CreateMap<SalesOrder, OrderDto>()
                .ForMember(dest => dest.Lines, opt => opt.MapFrom(src => src.Lines))
                .ReverseMap();
        }
    }
}
