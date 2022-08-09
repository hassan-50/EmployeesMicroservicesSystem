using EmployeeService.Dtos;

namespace EmployeeService.SyncDataServices;
public interface IHttpEmployeeDataClient {
    Task SendCreateEmployee(EmployeeCreateDto emp);
    Task SendUpdateEmployee(int id,EmployeeUpdateDto employee);
    Task SendDeleteEmployee(int id);
}