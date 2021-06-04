using AutoMapper;
using ParkyAPI.Models;
using ParkyAPI.Models.Dtos;

namespace ParkyAPI.Mapper
{
    public class Mappings : Profile
    {
        public Mappings()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
        }
    }
}
