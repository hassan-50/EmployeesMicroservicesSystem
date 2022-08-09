using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Kubernetes;

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

if(builder.Environment.IsDevelopment()){
builder.Configuration.AddJsonFile("Ocelot.Development.json");
}
else{    
builder.Configuration.AddJsonFile("Ocelot.Production.json");    
}
// Add services to the container.


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddOcelot().AddKubernetes();

builder.Services.AddAuthentication(opt =>
{
 opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
 opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
 options.TokenValidationParameters = new TokenValidationParameters
 {
 RequireExpirationTime = true,
 ValidateIssuer = true,
 ValidateAudience = true,
 ValidateLifetime = true,
 ValidateIssuerSigningKey = true,
 ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
 ValidAudience = builder.Configuration["JwtSettings:Audience"],
 IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.
GetBytes(builder.Configuration["JwtSettings:SecurityKey"]))
 };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AngularPolicy");
app.UseAuthentication();
app.UseWebSockets();
app.UseOcelot().Wait();

app.UseAuthorization();

app.MapControllers();

app.Run();
