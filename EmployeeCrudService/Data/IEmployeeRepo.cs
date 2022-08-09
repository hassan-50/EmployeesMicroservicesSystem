using EmployeeCrudService.Models;

namespace EmployeeCrudService.Data;
public interface IEmployeeRepo {
    bool SaveChanges();
    IEnumerable<Employee> GetAllEmployees();
    Employee GetEmployeeById(int id);
    void CreateEmployee(Employee employee);
    void UpdateEmployee();
    void DeleteEmployee(Employee employee);
    bool EmployeeNameExist(string employeeName);
}