using AutoMapper;
using EmployeeCrudService.Models;

namespace EmployeeCrudService.Data;
public class EmployeeRepo : IEmployeeRepo
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public EmployeeRepo(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public void CreateEmployee(Employee employee)
    {
        if(employee == null){
            throw new ArgumentNullException(nameof(employee));
        }
        _context.Employees.Add(employee);
    }

    public void DeleteEmployee(Employee employee)
    {        
        _context.Employees.Remove(employee);
    }

    public void UpdateEmployee()
    {
        // var employeeModal = GetEmployeeById(id);
        // _mapper.Map(employee, employeeModal);
        _context.SaveChanges();
    }
    public bool SaveChanges()
    {
        return (_context.SaveChanges() >= 0);
    }

    public IEnumerable<Employee> GetAllEmployees()
    {
        return _context.Employees.ToList();
    }

    public Employee GetEmployeeById(int id)
    {
        return _context.Employees.FirstOrDefault(E => E.Id == id)!;
    }

    public bool EmployeeNameExist(string employeeName)
    {
        return _context.Employees.Any(e=> e.Name == employeeName);
    }
}