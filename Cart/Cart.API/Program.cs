using Cart.API.Middlewares;
using Cart.BLL.Interfaces;
using Cart.BLL.Interfaces.Handler;
using Cart.BLL.Interfaces.Management;
using Cart.BLL.Mapping;
using Cart.BLL.Messaging.Events.Interfaces;
using Cart.BLL.Messaging.Events.Interfaces.ProductValidator;
using Cart.BLL.Messaging.Events.Services;
using Cart.BLL.Messaging.Events.Services.ProductValidator;
using Cart.BLL.Services;
using Cart.BLL.Services.Handler;
using Cart.BLL.Services.Management;
using Cart.DAL.Context;
using Cart.DAL.Interfaces;
using Cart.DAL.Interfaces.Handler;
using Cart.DAL.Interfaces.Management;
using Cart.DAL.Repositories;
using Cart.DAL.Repositories.Handler;
using Cart.DAL.Repositories.Management;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    c.AddSecurityDefinition(
        "Bearer",
        new OpenApiSecurityScheme
        {
            In = ParameterLocation.Header,
            Description = "Enter JWT token in format: Bearer {your_token}",
            Name = "Authorization",
            Type = SecuritySchemeType.Http,
            Scheme = "Bearer"
        }
    );

    c.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                },
                new string[] { }
            }
        }
    );
});

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<ICartManagementRepository, CartManagementRepository>();
builder.Services.AddScoped<ICartManagementService, CartManagementService>();
builder.Services.AddScoped<ICartHandlerRepository, CartHandlerRepository>();
builder.Services.AddScoped<ICartHandlerService, CartHandlerService>();

builder.Services.AddScoped<IProductValidatorPublisher, ProductValidatorPublisher>();

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddSingleton<IBuyerIdentificationPublisher, BuyerIdentificationPublisher>();
builder.Services.AddScoped<TaskCompletionSource<long>>();

builder.Services.AddAutoMapper(typeof(MappingProfile));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<JwtMiddleware>();

app.UseHttpsRedirection();

app.MapControllers();


app.Run();

