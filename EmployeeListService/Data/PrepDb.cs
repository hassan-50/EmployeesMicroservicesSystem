using EmployeeListService.Models;

namespace EmployeeListService.Data;

public static class PrebDb {

    public static void PrepPopulation(IApplicationBuilder app , bool isProd){
        using (var serviceScope = app.ApplicationServices.CreateScope())
        {
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>()! , isProd);
        }
    }

    private static void SeedData(AppDbContext context, bool isProd)
    {
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