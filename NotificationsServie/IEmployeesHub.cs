
namespace NotificationsServie.SignalR;
public interface IEmployeesHub  {
    Task BroadcastMessage(string notifications);    
}