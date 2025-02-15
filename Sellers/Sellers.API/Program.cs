using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Sellers.API.Middlewares;
using Sellers.BLL.Interfaces;
using Sellers.BLL.Interfaces.Filters;
using Sellers.BLL.Mapping;
using Sellers.BLL.Messaging.Background;
using Sellers.BLL.Messaging.Events.Interfaces;
using Sellers.BLL.Messaging.Events.Services;
using Sellers.BLL.Services;
using Sellers.BLL.Services.Filters;
using Sellers.DAL.Context;
using Sellers.DAL.Interfaces;
using Sellers.DAL.Interfaces.Filters;
using Sellers.DAL.Repositories;
using Sellers.DAL.Repositories.Filters;

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

builder.Services.AddScoped<IProfileRepository, ProfileRepository>();
builder.Services.AddScoped<IProfileService, ProfileService>();
builder.Services.AddScoped<IProfileManagementRepository, ProfileManagementRepository>();
builder.Services.AddScoped<IProfileManagementService, ProfileManagementService>();
builder.Services.AddScoped<ITransferUserToSellerEvent, TransferUserToSellerEvent>();
builder.Services.AddSingleton<IHostedService, TransferUserToSellerBackgroundService>();
builder.Services.AddScoped<ISellersFiltersRepository, SellersFiltersRepository>();
builder.Services.AddScoped<ISellersFiltersService, SellersFiltersService>();



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

