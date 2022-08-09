using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationService.Data;
public class AppDbContext : IdentityDbContext<ApplicationUser> {
public AppDbContext(DbContextOptions<AppDbContext> opt) :base(opt)
{
    
}

}