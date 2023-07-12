using Microsoft.EntityFrameworkCore;
using OnlineShopBackend.Data;
using OnlineShopBackend.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

//Database
var dbPath = "myapp.db";
builder.Services.AddDbContext<AppDbContext>(
   options => options.UseSqlite($"Data Source={dbPath}"));


var app = builder.Build();

app.UseCors(policy =>
{
	policy
		.AllowAnyMethod()
		.AllowAnyHeader()
		.AllowAnyOrigin();
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
app.MapGet("/get_products", async (AppDbContext context)
   => await context.Products.ToListAsync());

//R
app.MapGet("/get_product", GetProductById);

//U
app.MapPost("/update_product", UpdateProduct);

//D
app.MapPost("/delete_product", DeleteProduct);


async Task AddProduct(AppDbContext db, Product product)
{
	await db.Products.AddAsync(product);
	await db.SaveChangesAsync();
}

async Task<Product> GetProductById(AppDbContext db, Guid productId)
{
	var prod = await db.Products.FirstAsync(p => p.Id == productId);
	return prod;
}


async Task UpdateProduct(HttpContext context, AppDbContext db, Guid productId, Product product)
{
	var prod = await db.Products.SingleOrDefaultAsync(p => p.Id == productId);
	if(prod!=null)
	{
		db.Entry(prod).CurrentValues.SetValues(product);
		await db.SaveChangesAsync();
	}
	else
	{
		context.Response.StatusCode = 404;
	}

}

async Task DeleteProduct(HttpContext context, AppDbContext db, Guid productId)
{
	var prod = await db.Products.SingleOrDefaultAsync(p => p.Id == productId);
	if(prod != null)
	{
		db.Products.Remove(prod);
		await db.SaveChangesAsync();
	}
	else
	{
		context.Response.StatusCode = 404;
	}
}


app.Run();
