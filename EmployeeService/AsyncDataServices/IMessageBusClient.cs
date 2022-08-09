

using EmployeeService.Dtos;

namespace EmployeeService.AsyncDataServices;
public interface IMessageBusClient 
{
void PublishEmployee(EmployeePublishedDto employeePublishedDto);
}