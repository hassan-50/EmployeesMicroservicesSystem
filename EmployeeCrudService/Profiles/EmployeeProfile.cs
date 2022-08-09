using AutoMapper;
using EmployeeCrudService.Models;
using EmployeeCrudService.Dtos;

namespace EmployeeCrudService.Profiles;
public class EmployeeProfile : Profile {
public EmployeeProfile()
{
    CreateMap<EmployeeCreateDto,Employee>();
    CreateMap<EmployeeUpdateDto,Employee>();
    CreateMap<Employee,EmployeeReadDto>();
    CreateMap<EmployeePublishedDto,Employee>();    
}

}