using AutoMapper;
using EmployeeListService.Data;
using EmployeeListService.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCrudService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase {
    private readonly IEmployeeRepo _repository;
    private readonly IMapper _mapper;

    public EmployeesController(IEmployeeRepo repository, IMapper mapper )
    {
        _repository = repository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<EmployeeReadDto>>> GetEmployees(){
        Console.WriteLine("--> Getting Employees.....");

        var employees = await _repository.GetAllEmployees();

        return Ok(_mapper.Map<List<EmployeeReadDto>>(employees));
    }   

    [HttpGet("{id}")]
    public async Task<ActionResult<EmployeeReadDto>> GetEmployee(int id){
        Console.WriteLine("--> Getting Employee.....");

        var employee = await _repository.GetEmployee(id);

        return Ok(_mapper.Map<EmployeeReadDto>(employee));
    } 
}