using AutoMapper;
using EmployeeCrudService.AsyncDataServices;
using EmployeeCrudService.Data;
using EmployeeCrudService.Dtos;
using EmployeeCrudService.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeCrudService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase {
    private readonly IEmployeeRepo _repository;
    private readonly IMapper _mapper;
    private readonly IMessageBusClient _messageBusClient;

    public EmployeesController(IEmployeeRepo repository, IMapper mapper, IMessageBusClient messageBusClient )
    {
        _repository = repository;
        _mapper = mapper;
        _messageBusClient = messageBusClient;
    }

    [HttpGet]
    public ActionResult<IEnumerable<EmployeeReadDto>> GetEmployees(){
        Console.WriteLine("--> Getting Employees.....");

        var employees = _repository.GetAllEmployees();

        return Ok(_mapper.Map<List<EmployeeReadDto>>(employees));
    }

    [HttpGet("id",Name="GetEmployeeById")]
    public ActionResult<IEnumerable<Employee>> GetEmployeeById(int id){
        var employee = _repository.GetEmployeeById(id);

        if(employee != null)
        {
            return Ok(_mapper.Map<EmployeeReadDto>(employee));
        }
        return NotFound();

    }

    [HttpPost]
    public ActionResult<EmployeeReadDto> CreateEmployee(EmployeeCreateDto employee){
         var employeeModal = _mapper.Map<Employee>(employee);
        _repository.CreateEmployee(employeeModal);
        _repository.SaveChanges();

        // Send Async Message
        try
        {
            var notificationPublishedDto = new NotificationPublishedDto {
                 Event = "System_Update",PayloadMsg="Employee Created Successfully!"};                        
             _messageBusClient.PublishNotification(notificationPublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not send System Update Status Asynchronously: {e.Message}");
        }

        var employeeReadDto = _mapper.Map<EmployeeReadDto>(employeeModal);
        return CreatedAtRoute(nameof(GetEmployeeById), new {Id = employeeReadDto.Id}, employeeReadDto);
    }

    [HttpPut("{id}")]
    public ActionResult<EmployeeReadDto> UpdateEmployee(int id,EmployeeUpdateDto employee){
        employee.Id = id;
        var employeeModal = _repository.GetEmployeeById(id);
        
        if(employeeModal == null)
        {
            return NotFound();
        }
        _mapper.Map(employee,employeeModal);
        _repository.UpdateEmployee();

        try
        {
            var notificationPublishedDto = new NotificationPublishedDto {
                 Event = "System_Update",PayloadMsg="Employee Updated Successfully!"};                        
             _messageBusClient.PublishNotification(notificationPublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not send System Update Status Asynchronously: {e.Message}");
        }
        
        return Ok( _mapper.Map<EmployeeReadDto>(employeeModal));
    }

    [HttpDelete("{id}")]
    public ActionResult DeleteEmployee(int id){        
        var employeeModal = _repository.GetEmployeeById(id);

        if(employeeModal == null)
        {
            return NotFound();
        }        
        _repository.DeleteEmployee(employeeModal);
        _repository.SaveChanges();

        try
        {
            var notificationPublishedDto = new NotificationPublishedDto {
                 Event = "System_Update",PayloadMsg="Employee Deleted Successfully!"};                        
             _messageBusClient.PublishNotification(notificationPublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not send System Update Status Asynchronously: {e.Message}");
        }

        return Ok("Deleted Successfully");
    }

}