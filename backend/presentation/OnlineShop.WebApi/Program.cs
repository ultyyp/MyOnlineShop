using Microsoft.EntityFrameworkCore;
using OnlineShop.Data.EntityFramework;
using OnlineShop.Domain.Interfaces;
using OnlineShop.Data.EntityFramework.Repositories;
using OnlineShop.Domain.Services;
using Microsoft.AspNetCore.Identity;
using IdentityPasswordHasherLib;
using Microsoft.AspNetCore.HttpLogging;
using OnlineShop.WebApi.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using OnlineShop.WebApi.Configurations;
using OnlineShop.WebApi.Services;
using Microsoft.OpenApi.Models;
using OnlineShop.WebApi.Filters;
using OnlineShop.Domain.Events;
using Microsoft.Extensions.Configuration;
using OnlineShop.EmailSender.NotiSend;
using OnlineShop.EmailSender.SendGrid;

var builder = WebApplication.CreateBuilder(args);

JwtConfig jwtConfig = builder.Configuration
   .GetRequiredSection("JwtConfig")
   .Get<JwtConfig>()!;


if (jwtConfig is null)
{
	throw new InvalidOperationException("JwtConfig is not configured");
}
builder.Services.AddSingleton(jwtConfig);
builder.Services.AddSingleton<ITokenService, TokenService>();

// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.Filters.Add<CentralizedExceptionHandlingFilter>(order: 1);
    options.Filters.Add<AuthentificationRequestFilter>(order: 0);
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSwaggerGen(c =>
{
	c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
	{
		Name = "Authorization",
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
	});
	c.AddSecurityRequirement(new OpenApiSecurityRequirement {
	   {
		   new OpenApiSecurityScheme {
			   Reference = new OpenApiReference {
				   Type = ReferenceType.SecurityScheme,
				   Id = "Bearer"
			   }
		   },
		   Array.Empty<string>()
	   }
   });
});

builder.Services.AddCors();

builder.Services.AddAuthentication(options =>
{
	options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
	options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
   .AddJwtBearer(options =>
   {
	   options.TokenValidationParameters = new TokenValidationParameters
	   {
		   IssuerSigningKey = new SymmetricSecurityKey(jwtConfig.SigningKeyBytes),
		   ValidateIssuerSigningKey = true,
		   ValidateLifetime = true,
		   RequireExpirationTime = true,
		   RequireSignedTokens = true,

		   ValidateAudience = true,
		   ValidateIssuer = true,
		   ValidAudiences = new[] { jwtConfig.Audience },
		   ValidIssuer = jwtConfig.Issuer
	   };
   });
builder.Services.AddAuthorization();

//builder.Services.AddOptions<NotiSendConfig>()
//	.BindConfiguration("NotiSendConfig")
//	.ValidateDataAnnotations()
//	.ValidateOnStart();

builder.Services.AddOptions<SendGridConfig>()
    .BindConfiguration("SendGridConfig")
    .ValidateDataAnnotations()
    .ValidateOnStart();

//Database
builder.Services.AddScoped<IProductRepository, ProductRepositoryEf>();
builder.Services.AddScoped<IAccountRepository, AccountRepositoryEf>();
builder.Services.AddScoped<ICartRepository, CartRepositoryEf>();
builder.Services.AddScoped<IConfirmationCodeRepository, ConfirmationCodeRepositoryEf>();
builder.Services.AddScoped<IEmailSender, SendGridEmailSender>();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(typeof(AccountRegistered).Assembly);
    cfg.RegisterServicesFromAssemblies(typeof(SendLogin2FA).Assembly);
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWorkEf>();

builder.Services.AddScoped<AccountService>();
builder.Services.AddScoped<CartService>();
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

app.UseMiddleware<PageRequestCounterMiddleware>();


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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
