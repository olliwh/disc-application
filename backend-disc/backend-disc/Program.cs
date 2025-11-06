using backend_disc.Repositories;
using backend_disc.Services;
using class_library_disc.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowAll",
                              policy =>
                              {
                                  policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();

                              });
    options.AddPolicy(name: "AllowSome",
                              policy =>
                              {
                                  policy.WithOrigins("http://zealand.dk").WithMethods("Post", "Put").SetPreflightMaxAge(TimeSpan.FromSeconds(1440)).AllowAnyHeader();

                              });
    options.AddPolicy(name: "OnlyGET",
                              policy =>
                              {
                                  policy.AllowAnyOrigin()
                                  .WithMethods("GET")
                                  .AllowAnyHeader();
                              });
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add DbContext
builder.Services.AddDbContext<DiscProfileDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IEmployeesRepository, EmployeesRepository>();
builder.Services.AddScoped<IEmployeeService, EmployeeService>();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

//cors
app.UseCors("AllowAll");

app.MapControllers();


await app.RunAsync();
