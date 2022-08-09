
using EmployeeListService.Models;

namespace EmployeeListService.Data;
public interface IEmployeeRepo {
    Task<IEnumerable<Employee>> GetAllEmployees();    
    Task<Employee> GetEmployee(int id);    
}