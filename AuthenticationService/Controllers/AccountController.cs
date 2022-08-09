using System.IdentityModel.Tokens.Jwt;
using AuthenticationService.Data;
using AuthenticationService.Dtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace AuthenticationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JwtHandler _jwtHandler;
        private readonly IConfiguration _configuration;

        public AccountController(
 AppDbContext context,
 UserManager<ApplicationUser> userManager,
 JwtHandler jwtHandler, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _jwtHandler = jwtHandler;
            _configuration = configuration;

        }
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Email);
            if (user == null
            || !await _userManager.CheckPasswordAsync(user, loginRequest.
           Password))
                return Unauthorized(new LoginResult()
                {
                    Success = false,
                    Message = "Invalid Email or Password."
                });
            var secToken = await _jwtHandler.GetTokenAsync(user);
            var jwt = new JwtSecurityTokenHandler().WriteToken(secToken);
            return Ok(new LoginResult()
            {
                Success = true,
                Message = "Login successful",
                Token = jwt
            });
        }
        [HttpGet]
        public async Task<ActionResult> CreateDefaultUsers()
        {
            // setup the default role names
            string role_RegisteredUser = "RegisteredUser";
            string role_Administrator = "Administrator";
            // create the default roles (if they don't exist yet)
            if (await _roleManager.FindByNameAsync(role_RegisteredUser) ==  null)
                await _roleManager.CreateAsync(new IdentityRole(role_RegisteredUser));
            if (await _roleManager.FindByNameAsync(role_Administrator) == null)
                await _roleManager.CreateAsync(new IdentityRole(role_Administrator));
            // create a list to track the newly added users
            var addedUserList = new List<ApplicationUser>();
            // check if the admin user already exists
            var email_Admin = "admin@email.com";
            if (await _userManager.FindByNameAsync(email_Admin) == null)
            {
                // create a new admin ApplicationUser account
                var user_Admin = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = email_Admin,
                    Email = email_Admin,
                };
                // insert the admin user into the DB
                await _userManager.CreateAsync(user_Admin, _configuration["DefaultPasswords:Administrator"]);
                // assign the "RegisteredUser" and "Administrator" roles
                await _userManager.AddToRoleAsync(user_Admin,
                role_RegisteredUser);
                await _userManager.AddToRoleAsync(user_Admin,
                role_Administrator);
                // confirm the e-mail and remove lockout
                user_Admin.EmailConfirmed = true;
                user_Admin.LockoutEnabled = false;
                // add the admin user to the added users list
                addedUserList.Add(user_Admin);
            }
            // check if the standard user already exists
            var email_User = "user@email.com";
            if (await _userManager.FindByNameAsync(email_User) == null)
            {
                // create a new standard ApplicationUser account
                var user_User = new ApplicationUser()
                {
                    SecurityStamp = Guid.NewGuid().ToString(),
                    UserName = email_User,
                    Email = email_User
                };
                // insert the standard user into the DB
                await _userManager.CreateAsync(user_User,_configuration["DefaultPasswords:RegisteredUser"]);
                // assign the "RegisteredUser" role
                await _userManager.AddToRoleAsync(user_User,
                role_RegisteredUser);
                // confirm the e-mail and remove lockout
                user_User.EmailConfirmed = true;
                user_User.LockoutEnabled = false;
                // add the standard user to the added users list
                addedUserList.Add(user_User);
            }
            // if we added at least one user, persist the changes into the DB
            if (addedUserList.Count > 0)
                await _context.SaveChangesAsync();
            return new JsonResult(new
            {
                Count = addedUserList.Count,
                Users = addedUserList
            });
        }
    }
}