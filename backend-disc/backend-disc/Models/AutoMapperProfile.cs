using AutoMapper;
using backend_disc.Dtos.Companies;
using class_library_disc.Models;

namespace backend_disc.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            //company
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<CreateCompanyDto, Company>();
            CreateMap<UpdateCompanyDto, Company>();

            //company
            CreateMap<Position, PositionDto>().ReverseMap();
            CreateMap<CreatePositionDto, Position>();
            CreateMap<UpdatePositionDto, Position>();

            //Department
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<CreateDepartmentDto, Department>();
            CreateMap<UpdateDepartmentDto, Department>();

            //DiscProfile
            CreateMap<DiscProfile, DiscProfileDto>().ReverseMap();
            CreateMap<CreateDiscProfileDto, DiscProfile>();
            CreateMap<UpdateDiscProfileDto, DiscProfile>();
        }
    }
}
