using EmployeeCrudService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCrudService.Data;
public class AppDbContext : DbContext {
    public AppDbContext(DbContextOptions<AppDbContext> opt): base(opt)
    {
        
    }
    public DbSet<Employee> Employees { get; set; } = null!;

}