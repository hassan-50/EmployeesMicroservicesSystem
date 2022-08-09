using System.Text.Json;
using AutoMapper;
using EmployeeCrudService.AsyncDataServices;
using EmployeeCrudService.Data;
using EmployeeCrudService.Dtos;
using EmployeeCrudService.Models;

namespace EmployeeCrudService.EventProcessing;
public class EventProcessor : IEventProcessor
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IMapper _mapper;
    private readonly IMessageBusClient _messageBusClient;

    public EventProcessor(IServiceScopeFactory scopeFactory, AutoMapper.IMapper mapper, IMessageBusClient messageBusClient)
    {
        _scopeFactory = scopeFactory;
        _mapper = mapper;
        _messageBusClient = messageBusClient;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.EmployeeCreate:
                addEmployee(message);
                break;
            case EventType.EmployeeUpdate:
                updateEmployee(message);
                break;
            case EventType.EmployeeDelete:
                deleteEmployee(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notifcationMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);

        switch (eventType?.Event)
        {
            case "Employee_Create":
                Console.WriteLine("--> Employee Create Event Detected");
                return EventType.EmployeeCreate;
            case "Employee_Update":
                Console.WriteLine("--> Employee Update Event Detected");
                return EventType.EmployeeUpdate;
            case "Employee_Delete":
                Console.WriteLine("--> Employee Delete Event Detected");
                return EventType.EmployeeDelete;
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
        }
    }

    private void addEmployee(string employeePublishedMessage)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<IEmployeeRepo>();

            var employeePublishedDto = JsonSerializer.Deserialize<EmployeePublishedDto>(employeePublishedMessage);
            if(repo.EmployeeNameExist(employeePublishedDto?.Name!)) {
                SendErrorNotification("Employee Already Exist!");
            }
            else{
            try
            {
                var emp = _mapper.Map<Employee>(employeePublishedDto);

                repo.CreateEmployee(emp);
                repo.SaveChanges();
                Console.WriteLine("--> Employee added!");
                SendUpdateNotification("Employee Created Successfully!");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not add Employee to DB {ex.Message}");
            }
            }
        }
    }

    private void updateEmployee(string employeePublishedMessage)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<IEmployeeRepo>();

            var platformPublishedDto = JsonSerializer.Deserialize<EmployeePublishedDto>(employeePublishedMessage);

            try
            {
                Console.WriteLine(platformPublishedDto?.Id);
                var employeeModal = repo.GetEmployeeById(platformPublishedDto!.Id);
                if (employeeModal != null)
                {
                    _mapper.Map(platformPublishedDto, employeeModal);
                    repo.UpdateEmployee();
                    Console.WriteLine("--> Employee updated!");
                    SendUpdateNotification("Employee Updated Successfully!");
                }
                else
                {
                    SendErrorNotification("Employee Not Found!");                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not Update Employee to DB {ex.Message}");
            }
        }
    }

    private void deleteEmployee(string employeePublishedMessage)
    {
        using (var scope = _scopeFactory.CreateScope())
        {
            var repo = scope.ServiceProvider.GetRequiredService<IEmployeeRepo>();

            var platformPublishedDto = JsonSerializer.Deserialize<EmployeePublishedDto>(employeePublishedMessage);

            try
            {
                var employeeModal = repo.GetEmployeeById(platformPublishedDto!.Id);
                if (employeeModal != null)
                {
                    _mapper.Map(platformPublishedDto, employeeModal);
                    repo.DeleteEmployee(employeeModal);
                    repo.SaveChanges();
                    Console.WriteLine("--> Employee Deleted!");
                    SendUpdateNotification("Employee Deleted Successfully!");
                }
                else
                {
                     SendErrorNotification("Employee Not Found!");                    
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not Delete Employee From DB {ex.Message}");
            }
        }
    }

    private void SendUpdateNotification(string notifiationMsg){
        try
        {
            var notificationPublishedDto = new NotificationPublishedDto {
                 Event = "System_Update",PayloadMsg=notifiationMsg};                        
             _messageBusClient.PublishNotification(notificationPublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not send System Update Status Asynchronously: {e.Message}");
        }
    }

    private void SendErrorNotification(string notifiationMsg){
        try
        {
            var notificationPublishedDto = new NotificationPublishedDto {
                 Event = "System_Error",PayloadMsg=notifiationMsg};                        
             _messageBusClient.PublishNotification(notificationPublishedDto);
        }
        catch (Exception e)
        {
            Console.WriteLine($"--> Could Not send System Error Status Asynchronously: {e.Message}");
        }
    }

}

enum EventType
{
    EmployeeCreate,
    EmployeeUpdate,
    EmployeeDelete,
    Undetermined
}