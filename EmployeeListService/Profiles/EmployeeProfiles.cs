using AutoMapper;
using EmployeeListService.Dtos;
using EmployeeListService.Models;

namespace EmployeeListService.Profiles;

public class EmployeeProfiles : Profile
{
    public EmployeeProfiles()
    {
        CreateMap<Employee,EmployeeReadDto>();
    }
}