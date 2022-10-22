using Microsoft.EntityFrameworkCore;
using NewsParserApi.Data;
using NewsParserApi.Models;
using NewsParserApi.Repositories.Implementations;
using NewsParserApi.Repositories.Interfaces;
using NewsParserApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string connectionString;
if(Environment.GetEnvironmentVariable("DB_HOST") != null) 
{
    var dbHost = Environment.GetEnvironmentVariable("DB_HOST");
    var dbName = Environment.GetEnvironmentVariable("DB_NAME");
    var dbPass = Environment.GetEnvironmentVariable("DB_SA_PASSWORD");
    connectionString = $"Data Source={dbHost};Initial Catalog={dbName};User ID=sa;Password={dbPass}";
}
else
    connectionString = "Server=(localdb)\\mssqllocaldb;Database=NewsApi.Data;Trusted_Connection=True;MultipleActiveResultSets=true";

builder.Services.AddDbContext<NewsApiDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<INewsRepository, NewsRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddHostedService<TimedNewsParser>();

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
        policy =>
        {
            policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader().AllowCredentials().WithOrigins("http://localhost:8081");
        });
});

var app = builder.Build();

app.UseCors(MyAllowSpecificOrigins);
// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// app.UseAuthorization();

app.MapControllers();

app.Run();
