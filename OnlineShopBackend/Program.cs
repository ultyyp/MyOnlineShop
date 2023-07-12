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
		.WithOrigins("https://localhost:5001")
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
app.MapGet("/get_products", async (AppDbContext context)
   => await context.Products.ToListAsync());

//R
app.MapGet("/get_product", GetProductById);

//U
app.MapPost("/update_product", UpdateProduct);

//D
app.MapPost("/delete_product", DeleteProduct);


async Task AddProduct(AppDbContext db, Product product, CancellationToken cancellationToken)
{
	await db.Products.AddAsync(product, cancellationToken);
	await db.SaveChangesAsync(cancellationToken);
}

async Task<Product> GetProductById(AppDbContext db, Guid productId, CancellationToken cancellationToken)
{
	var prod = await db.Products.FirstAsync(p => p.Id == productId, cancellationToken);
	return prod;
}


async Task UpdateProduct(HttpContext context, AppDbContext db, Guid productId, Product product, CancellationToken cancellationToken)
{
	var prod = await db.Products.SingleOrDefaultAsync(p => p.Id == productId, cancellationToken);
	if(prod!=null)
	{
		db.Entry(prod).CurrentValues.SetValues(product);
		await db.SaveChangesAsync(cancellationToken);
	}
	else
	{
		context.Response.StatusCode = 404;
	}

}

async Task DeleteProduct(HttpContext context, AppDbContext db, Guid productId, CancellationToken cancellationToken)
{
	var prod = await db.Products.SingleOrDefaultAsync(p => p.Id == productId, cancellationToken);
	if(prod != null)
	{
		db.Products.Remove(prod);
		await db.SaveChangesAsync(cancellationToken);
	}
	else
	{
		context.Response.StatusCode = 404;
	}
}


app.Run();
