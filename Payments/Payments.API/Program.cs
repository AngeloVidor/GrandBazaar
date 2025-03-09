using DotNetEnv;
using Microsoft.OpenApi.Models;
using Payments.API.Middlewares;
using Payments.BLL.Interfaces;
using Payments.BLL.Interfaces.Cart;
using Payments.BLL.Messaging.Background;
using Payments.BLL.Messaging.Cart.Interfaces;
using Payments.BLL.Messaging.Cart.Services;
using Payments.BLL.Messaging.Product.Interfaces;
using Payments.BLL.Messaging.Product.Services;
using Payments.BLL.Services;
using Payments.BLL.Services.Cart;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Configuration.AddEnvironmentVariables();

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



builder.Services.AddSingleton<IHostedService, ServiceBackground>();
builder.Services.AddScoped<IStripeProductService, StripeProductService>();
builder.Services.AddSingleton<IStripeProductSubscriber, StripeProductSubscriber>();

builder.Services.AddSingleton<IProductRequestPublisher, ProductRequestPublisher>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IUserRequestHandler, UserRequestHandler>();


builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<PriceService>();


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

