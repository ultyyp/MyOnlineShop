using Microsoft.EntityFrameworkCore;
using OnlineShopBackend.Data;
using OnlineShopBackend.Entities;
using OnlineShopBackend.Interfaces;
using OnlineShopBackend.Repositories;

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

var dbPath = "myapp.db";
builder.Services.AddDbContext<AppDbContext>(
   options => options.UseSqlite($"Data Source={dbPath}"));


var app = builder.Build();

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
