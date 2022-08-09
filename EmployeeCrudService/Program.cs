using EmployeeCrudService.AsyncDataServices;
using EmployeeCrudService.Data;
using EmployeeCrudService.EventProcessing;
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
// if(builder.Environment.IsProduction()){
// Console.WriteLine("--> using sqlserver database");
// builder.Services.AddDbContext<AppDbContext>(opt=> 
// opt.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeConn")));    
// }else{
// Console.WriteLine("--> using InMem database");
// builder.Services.AddDbContext<AppDbContext>(opt=> opt.UseInMemoryDatabase("InMem"));
// }
builder.Services.AddDbContext<AppDbContext>(opt=> 
opt.UseSqlServer(builder.Configuration.GetConnectionString("EmployeeConn")));  

builder.Services.AddScoped<IEmployeeRepo,EmployeeRepo>();
builder.Services.AddHostedService<MessageBusSubscriber>();
builder.Services.AddSingleton<IEventProcessor,EventProcessor>();
builder.Services.AddSingleton<IMessageBusClient,MessageBusClient>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
Console.WriteLine($"--> Env -- {builder.Environment.EnvironmentName}");
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
