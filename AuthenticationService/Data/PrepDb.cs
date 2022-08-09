using AuthenticationService.Data;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Data;
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
    }
}