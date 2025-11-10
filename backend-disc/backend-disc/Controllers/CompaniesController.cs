using backend_disc.Dtos.Companies;
using backend_disc.Services;
using Microsoft.AspNetCore.Mvc;


namespace backend_disc.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompaniesController : GenericController<CompanyDto, CreateCompanyDto, UpdateCompanyDto>
    {
        public CompaniesController(IGenericService<CompanyDto, CreateCompanyDto, UpdateCompanyDto> service) : base(service) { }

    }
}
