using AutoMapper;
using backend_disc.Dtos.Companies;
using class_library_disc.Models;

namespace backend_disc.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Company, CompanyDto>().ReverseMap();


        }
    }
}
