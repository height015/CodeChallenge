using System.Text.Json;
using System.Text.Json.Serialization;
using CodeChallenge.Contracts;
using CodeChallenge.Data;
using CodeChallenge.Data.Repository;
using CodeChallenge.Domain;
using CodeChallenge.Service;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using WebApplication.Service;

var builder = Microsoft.AspNetCore.Builder.WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnections"));
});

///This

builder.Services.AddHttpClient(); // registers IHttpClientFactory
                                  // Add services to the container.

builder.Services.Configure<HttpClient>(options =>
{
    options.BaseAddress = new Uri("https://api.github.com/");
    options.DefaultRequestHeaders.Add(HeaderNames.Accept, "application/vnd.github.v3+json");
    options.DefaultRequestHeaders.Add(HeaderNames.UserAgent, "HttpRequestsSample");
});

builder.Services.AddLogging(options =>
{
    options.AddConsole();
    options.AddDebug();
});

//Repository
builder.Services.AddScoped<IRepository<Grade>, Repository<Grade>>();
builder.Services.AddScoped<IRepository<StudentCourse>, Repository<StudentCourse>>();
builder.Services.AddScoped<IRepository<Student>, Repository<Student>>();
builder.Services.AddScoped<IRepository<Courses>, Repository<Courses>>();



builder.Logging.SetMinimumLevel(LogLevel.Error);

builder.Services.AddScoped<IRandomUserService, RandomUserManager>();


builder.Services.AddScoped<IStudentService, StudentRepository>();
builder.Services.AddScoped<IStudentCourseService, StudentCourseRepository>();
builder.Services.AddScoped<IGradeService, GradeRepository>();
builder.Services.AddScoped<ICoursesService, CoursesRepository>();

builder.Services.AddHttpClient("YourApiClient", client =>
{
    // Configure HttpClient options
    client.BaseAddress = new Uri("https://randomuser.me/api/");
    // Add any additional configuration you need (headers, timeouts, etc.)
});


JsonSerializerOptions options = new()
{
    ReferenceHandler = ReferenceHandler.IgnoreCycles,
    WriteIndented = true
};

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

