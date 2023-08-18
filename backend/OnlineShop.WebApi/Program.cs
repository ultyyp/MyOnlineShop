using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.EntityFramework;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Data.EntityFramework.Repositories;
using OnlineShop.Domain.Services;
using Microsoft.AspNetCore.Identity;
using IdentityPasswordHasherLib;
using Microsoft.AspNetCore.HttpLogging;
using OnlineShop.WebApi.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

//Database
builder.Services.AddScoped<IProductRepository, ProductRepositoryEf>();
builder.Services.AddScoped<IAccountRepository, AccountRepositoryEf>();
builder.Services.AddScoped<AccountService>();
builder.Services.AddSingleton<IApplicationPasswordHasher, IdentityPasswordHasher>();
builder.Services.AddSingleton<IPageRequestCounterService, PageRequestCounterService>();

//Logging
builder.Services.AddHttpLogging(options =>
{
	options.LoggingFields = HttpLoggingFields.RequestHeaders |
							HttpLoggingFields.RequestBody |
							HttpLoggingFields.ResponseHeaders |
							HttpLoggingFields.ResponseBody;
});

var dbPath = "myapp.db";
builder.Services.AddDbContext<AppDbContext>(
   options => options.UseSqlite($"Data Source={dbPath}"));


var app = builder.Build();

app.UseMiddleware<PathRequestCounterMiddleware>();


app.UseCors(policy =>
{
	policy
		.WithOrigins("https://localhost:5001", "https://localhost:7181")
		.AllowAnyHeader()
		.AllowAnyMethod();
});


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
