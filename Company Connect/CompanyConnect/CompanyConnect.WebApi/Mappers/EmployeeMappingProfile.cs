using AutoMapper;
using CompanyConnect.Core.Domain.Entities;
using CompanyConnect.Core.Dto;

namespace CompanyConnect.WebApi.Mappers;

public class EmployeeMappingProfile : Profile
{
    public EmployeeMappingProfile()
    {
        CreateMap<CreateEmployeeDto, Employee>();
    }
}