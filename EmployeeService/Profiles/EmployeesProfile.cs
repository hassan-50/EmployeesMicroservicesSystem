using AutoMapper;
using EmployeeService.Dtos;

namespace PlatformService.Profiles;
public class PlatformsProfile : Profile{
    public PlatformsProfile()
    {
        CreateMap<EmployeeCreateDto, EmployeePublishedDto>();
        CreateMap<EmployeeUpdateDto, EmployeePublishedDto>();
                   
    }
}