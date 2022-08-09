using AuthenticationService.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AngularPolicy",
                      policy  =>
                      {
                          policy.AllowAnyHeader();
                          policy.AllowAnyMethod();
                          policy.AllowAnyOrigin();
                      });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("UsersConn")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
 options.SignIn.RequireConfirmedAccount = true;
 options.Password.RequireDigit = true;
 options.Password.RequireLowercase = true;
 options.Password.RequireUppercase = true;
 options.Password.RequireNonAlphanumeric = true;
 options.Password.RequiredLength = 8;
})
 .AddEntityFrameworkStores<AppDbContext>();
builder.Services.AddScoped<JwtHandler>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");

app.UseAuthorization();

app.MapControllers();

PrepDb.PropPopulation(app);

app.Run();
