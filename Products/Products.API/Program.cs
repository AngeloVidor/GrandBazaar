using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Products.API.Middlewares;
using Products.BLL.Interfaces;
using Products.BLL.Interfaces.Filters;
using Products.BLL.Interfaces.Provider;
using Products.BLL.Mapping;
using Products.BLL.Messaging.Events.Interfaces;
using Products.BLL.Messaging.Events.Services;
using Products.BLL.Services;
using Products.BLL.Services.Filters;
using Products.BLL.Services.Provider;
using Products.DAL.Context;
using Products.DAL.Interfaces;
using Products.DAL.Interfaces.Filters;
using Products.DAL.Interfaces.Provider;
using Products.DAL.Repositories;
using Products.DAL.Repositories.Filters;
using Products.DAL.Repositories.Provider;

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

builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ITransferUserToSellerEvent, TransferUserToSellerEvent>();
builder.Services.AddScoped<TaskCompletionSource<long>>();
builder.Services.AddScoped<IProductProviderRepository, ProductProviderRepository>();
builder.Services.AddScoped<IProductProviderService, ProductProviderService>();
builder.Services.AddScoped<IProductFilterRepository, ProductFilterRepository>();
builder.Services.AddScoped<IProductFilterService, ProductFilterService>();



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

