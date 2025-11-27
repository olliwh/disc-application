using AutoMapper;
using backend_disc.Dtos.Departments;
using backend_disc.Dtos.Positions;
using backend_disc.Dtos.DiscProfiles;
using backend_disc.Dtos.Employees;
using backend_disc.Repositories.StoredProcedureParams;
using class_library_disc.Models.Sql;

namespace backend_disc.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
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

            //Employee
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<CreateNewEmployee, AddEmployeeSpParams>().ReverseMap();
            CreateMap<EmployeesOwnProfile, EmployeeOwnProfileDto>().ReverseMap();


   



        }
    }
}
