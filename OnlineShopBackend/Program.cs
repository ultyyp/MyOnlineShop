using Microsoft.EntityFrameworkCore;
using OnlineShopBackend.Data;
using OnlineShopBackend.Entities;
using OnlineShopBackend.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

//Database
builder.Services.AddScoped<IProductRepository, ProductRepositoryEf>();	

var dbPath = "myapp.db";
builder.Services.AddDbContext<AppDbContext>(
   options => options.UseSqlite($"Data Source={dbPath}"));


var app = builder.Build();

app.UseCors(policy =>
{
	policy
		.WithOrigins("https://localhost:5001", "https://localhost:7181")
        .AllowAnyMethod()
		.AllowAnyHeader();
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


//C
app.MapPost("/add_product", AddProduct);

//R
app.MapGet("/get_products", GetProducts);

//R
app.MapGet("/get_product", GetProductById);

//U
app.MapPost("/update_product", UpdateProduct);

//D
app.MapPost("/delete_product", DeleteProduct);


async Task AddProduct(IProductRepository repository, Product product, CancellationToken cancellationToken)
{
	await repository.AddProduct(product, cancellationToken);
}

async Task<List<Product>> GetProducts(IProductRepository repository, CancellationToken cancellationToken)
{
	return await repository.GetProducts(cancellationToken);
}

async Task<Product> GetProductById(IProductRepository repository, Guid productId, CancellationToken cancellationToken)
{
	return await repository.GetProductById(productId, cancellationToken);
}


async Task UpdateProduct(IProductRepository repository, Guid productId, Product product, CancellationToken cancellationToken)
{
	await repository.UpdateProductById(productId, product, cancellationToken);
}

async Task DeleteProduct(IProductRepository repository, Guid productId, CancellationToken cancellationToken)
{
	await repository.DeleteProductById(productId, cancellationToken);
}


app.Run();
