using backend_disc.Dtos.Departments;
using backend_disc.Dtos.DiscProfiles;
using backend_disc.Dtos.Positions;
using backend_disc.Models;
using backend_disc.Factories;
using backend_disc.Repositories;
using backend_disc.Repositories.Mongo;
using backend_disc.Repositories.Neo4J;
using backend_disc.Services;
using class_library_disc.Data;
using class_library_disc.Models.Sql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Neo4j.Driver;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

// Add logging configuration
builder.Services.AddLogging(config =>
{
    config.AddConsole();
    config.AddDebug();
    config.SetMinimumLevel(LogLevel.Debug);
});

//Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowFrontend",
                              policy =>
                              {
                                  policy
            .WithOrigins(
                "http://localhost:3000",
                "https://disc-application-fronttend.onrender.com"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();

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
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Authentication",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };
    options.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, Array.Empty<string>()}
    });
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<DiscProfileDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddAutoMapper(
    cfg => { }, // optional config lambda 
    typeof(AutoMapperProfile) // where to find mappers
);
// Neo4j driver registration
builder.Services.AddSingleton<IDriver>(provider =>
{
    var config = builder.Configuration;

    return GraphDatabase.Driver(
        config["Neo4j:Uri"],
        AuthTokens.Basic(config["Neo4j:User"], config["Neo4j:Password"])
    );
});
builder.Services.AddSingleton<IMongoClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connString = configuration["MongoDb:ConnectionString"];
    return new MongoClient(connString);
});


builder.Services.AddScoped<IGenericService<DepartmentDto, CreateDepartmentDto, UpdateDepartmentDto>,
    GenericService<Department, DepartmentDto, CreateDepartmentDto, UpdateDepartmentDto>>();
builder.Services.AddScoped<IGenericService<DiscProfileDto, CreateDiscProfileDto, UpdateDiscProfileDto>,
    GenericService<DiscProfile, DiscProfileDto, CreateDiscProfileDto, UpdateDiscProfileDto>>();
builder.Services.AddScoped<IGenericService<PositionDto, CreatePositionDto, UpdatePositionDto>,
    GenericService<Position, PositionDto, CreatePositionDto, UpdatePositionDto>>();
builder.Services.AddHttpClient<IWeatherService, WeatherService>();

builder.Services.AddScoped<IGenericRepositoryFactory, GenericRepositoryFactory>();
builder.Services.AddScoped(typeof(GenericRepository<>));
builder.Services.AddScoped(typeof(GenericMongoRepository<>));
builder.Services.AddScoped(typeof(GenericNeo4JRepository<>));

builder.Services.AddScoped<IEmployeesRepository, EmployeesMongoRepository>();
builder.Services.AddScoped<IEmployeeRepositoryFactory, EmployeeRepositoryFactory>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<EmployeesRepository>();
builder.Services.AddScoped<EmployeesMongoRepository>();
builder.Services.AddScoped<EmployeesNeo4JRepository>();
builder.Services.AddScoped<IGenericRepository<Department>, DepartmentsNeo4JRepository>();


builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var secretKey = builder.Configuration["API_SECRET_KEY"]
    ?? throw new InvalidOperationException("API_SECRET_KEY is not configured");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(jwtOptions =>
{
    jwtOptions.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ValidateIssuer = false, 
        ValidateAudience = false, 
        ValidateLifetime = true, 
        ClockSkew = TimeSpan.Zero // Removes default 5-minute tolerance
    };
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// CORS must be placed before Authorization and HttpsRedirection
app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Only create tables in development
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var db = scope.ServiceProvider.GetRequiredService<DiscProfileDbContext>();
        await db.Database.EnsureCreatedAsync();
    }
}

await app.RunAsync();
