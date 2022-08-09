using System.Text.Json;
using EmployeeCrudService.Dtos;
using Microsoft.AspNetCore.SignalR;
using NotificationsServie.Dtos;
using NotificationsServie.SignalR;

namespace NotificationsServie.EventProcessing;
public class EventProcessor : IEventProcessor
{        
    private readonly IHubContext<EmployeesHub> _hub;

    public EventProcessor(IHubContext<EmployeesHub> hub)
    {
        _hub = hub;
    }

    public void ProcessEvent(string message)
    {
        var eventType = DetermineEvent(message);

        switch (eventType)
        {
            case EventType.SystemUpdate:
                sendNotification(message);
                break;
            case EventType.SystemError:
                sendErrorNotification(message);
                break;
            default:
                break;
        }
    }

    private EventType DetermineEvent(string notifcationMessage)
    {
        Console.WriteLine("--> Determining Event");

        var eventType = JsonSerializer.Deserialize<GenericEventDto>(notifcationMessage);
        Console.WriteLine(eventType?.Event);
        switch (eventType?.Event)
        {
            case "System_Update":
                Console.WriteLine("--> System Update Event Detected");
                return EventType.SystemUpdate;  
            case "System_Error":
                Console.WriteLine("--> System Error Event Detected");
                return EventType.SystemError;           
            default:
                Console.WriteLine("--> Could not determine the event type");
                return EventType.Undetermined;
        }
    }

    private async void sendNotification(string notificationPublishedMessage)
    {
        var notificationPublishedDto = JsonSerializer.Deserialize<NotificationPublishedDto>(notificationPublishedMessage);
        Console.WriteLine(notificationPublishedDto?.PayloadMsg);
        await _hub.Clients.All.SendAsync("Update", notificationPublishedDto?.PayloadMsg);                
    } 
    private async void sendErrorNotification(string notificationPublishedMessage)
    {
        var notificationPublishedDto = JsonSerializer.Deserialize<NotificationPublishedDto>(notificationPublishedMessage);
        Console.WriteLine(notificationPublishedDto?.PayloadMsg);
        await _hub.Clients.All.SendAsync("Error", notificationPublishedDto?.PayloadMsg);                
    }   
}

enum EventType
{
    SystemUpdate,
    SystemError,
    Undetermined
}