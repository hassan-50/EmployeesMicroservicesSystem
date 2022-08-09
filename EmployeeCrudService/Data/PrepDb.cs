using EmployeeCrudService.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeCrudService.Data;
public static class PrepDb
{
    public static void PropPopulation(IApplicationBuilder app )
    {
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()!);
        }
    }

    private static void SeedData(AppDbContext context)
    {
                Console.WriteLine("--> Attempting to apply migrations...");
                try
                {
                        context.Database.Migrate();
                }
                catch (Exception ex)
                {
                        Console.WriteLine($"--> Could Not Run Migrations : {ex.Message}");                                                
                }        
        if (!context.Employees.Any())
        {
            Console.WriteLine("--> Seeding Data");
            context.Employees.AddRange(
            new Employee { Name = "Hassan", Age = 24, Sex = 'M', job = "Software Engineer", Salary = 14000 },
            new Employee { Name = "Wafik", Age = 30, Sex = 'M', job = "Solutions Architect", Salary = 80000 },
            new Employee { Name = "Heba", Age = 23, Sex = 'F', job = "Graphic Designer", Salary = 12000 }
            );
            context.SaveChanges();

        }
        else
        {
            Console.WriteLine("--> Already Have Data");
        }
    }
}