using Amazon.S3;
using Buyers.API.Middlewares;
using Buyers.BLL.Interfaces;
using Buyers.BLL.Interfaces.Management;
using Buyers.BLL.Interfaces.S3;
using Buyers.BLL.Mapping;
using Buyers.BLL.Messaging.Background;
using Buyers.BLL.Messaging.Costumer.Interfaces;
using Buyers.BLL.Messaging.Costumer.Services;
using Buyers.BLL.Messaging.Events.Interfaces;
using Buyers.BLL.Messaging.Events.Services;
using Buyers.BLL.Services;
using Buyers.BLL.Services.Management;
using Buyers.BLL.Services.S3;
using Buyers.DAL.Context;
using Buyers.DAL.Interfaces;
using Buyers.DAL.Interfaces.Management;
using Buyers.DAL.Repositories;
using Buyers.DAL.Repositories.Management;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

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


builder.Services.AddScoped<IBuyerRepository, BuyerRepository>();
builder.Services.AddScoped<IBuyerService, BuyerService>();
builder.Services.AddScoped<IBuyerManagementRepository, BuyerManagementRepository>();
builder.Services.AddScoped<IS3StorageService, S3StorageService>();
builder.Services.AddScoped<IBuyerManagementService, BuyerManagementService>();

builder.Services.AddScoped<IBuyerIdentificationPublisher, BuyerIdentificationPublisher>();
builder.Services.AddSingleton<IUserIdentificationSub, UserIdentificationSub>();
//builder.Services.AddSingleton<IHostedService, Background>();
builder.Services.AddHostedService<Background>();


builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();


builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.Configure<AWSSettings>(options =>
{
    options.BucketName = Environment.GetEnvironmentVariable("AWS_BucketName");
    options.AccessKey = Environment.GetEnvironmentVariable("AWS_ACCESS_KEY_ID");
    options.SecretKey = Environment.GetEnvironmentVariable("AWS_SECRET_ACCESS_KEY");
    options.Region = Environment.GetEnvironmentVariable("AWS_REGION");
});


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

