using System;
using AutoMapper;
using EmployeeService.AsyncDataServices;
using EmployeeService.Dtos;
using EmployeeService.SyncDataServices;
using Microsoft.AspNetCore.Mvc;

namespace EmployeeService.Controllers;
[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IHttpEmployeeDataClient _employeeDataClient;
    private readonly IMessageBusClient _messageBusClient;
    private readonly IMapper _mapper;
    private readonly IConfiguration _config;

    public EmployeesController(IHttpEmployeeDataClient employeeDataClient, IMessageBusClient messageBusClient, IMapper mapper, IConfiguration config)
    {
        _employeeDataClient = employeeDataClient;
        _messageBusClient = messageBusClient;
        _mapper = mapper;
        _config = config;
    }
    [HttpPost]
    public async Task<ActionResult> CreateEmployee(EmployeeCreateDto emp)
    {
        if(_config["MessagingProtocol"] == "Asynchronous"){
        // Send Async Message
        try
        {
            var employeePublishedDto = _mapper.Map<EmployeePublishedDto>(emp);
            employeePublishedDto.Event = "Employee_Create";
            _messageBusClient.PublishEmployee(employeePublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not send Create Employee Asynchronously: {e.Message}");
        }
        }else{
        // Send Sync Message 
        try
        {
            await _employeeDataClient.SendCreateEmployee(emp);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not send Create Employee Synchronously: {e.Message}");
        }
        }        

        return Ok();
    }
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateEmployee(int id, EmployeeUpdateDto emp)
    {

        if(_config["MessagingProtocol"] == "Asynchronous"){
        // Send Async Message
        try
        {
            var employeePublishedDto = _mapper.Map<EmployeePublishedDto>(emp);
            employeePublishedDto.Event = "Employee_Update";
            employeePublishedDto.Id = id;
            _messageBusClient.PublishEmployee(employeePublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not send Update Employee Asynchronously: {e.Message}");
        }
        } else {
        // Send sync Message
        try
        {
            await _employeeDataClient.SendUpdateEmployee(id, emp);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not send Update Employee Synchronously: {e.Message}");
        }
        }    
        return Ok();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteEmployee(int id)
    {
        if(_config["MessagingProtocol"] == "Asynchronous"){
        // Send Async Message
        try
        {
            var employeePublishedDto = new EmployeePublishedDto { Id = id };
            employeePublishedDto.Event = "Employee_Delete";
             _messageBusClient.PublishEmployee(employeePublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not send Delete Employee Asynchronously: {e.Message}");
        }
        }else {
        // Send Sync Message
        try
        {
            await _employeeDataClient.SendDeleteEmployee(id);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not send Delete Employee Synchronously: {e.Message}");
        }
        }
        return Ok();
    }

}