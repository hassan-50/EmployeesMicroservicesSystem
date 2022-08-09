using AutoMapper;
using EmployeeListService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeListService.Data;
public class EmployeeRepo : IEmployeeRepo
{
    private readonly AppDbContext _context;    
    public EmployeeRepo(AppDbContext context)
    {
        _context = context;    
    }
    public async Task<IEnumerable<Employee>> GetAllEmployees()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<Employee> GetEmployee(int id)
    {
        return await _context.Employees.FirstOrDefaultAsync(e=> e.Id == id);
    }
}