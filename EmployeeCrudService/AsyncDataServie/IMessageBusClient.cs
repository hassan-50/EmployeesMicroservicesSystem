using EmployeeCrudService.Dtos;

namespace EmployeeCrudService.AsyncDataServices;
public interface IMessageBusClient 
{
void PublishNotification(NotificationPublishedDto notificationPublishedDto);
}