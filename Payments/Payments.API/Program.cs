using DotNetEnv;
using Payments.BLL.Interfaces;
using Payments.BLL.Messaging.Background;
using Payments.BLL.Messaging.Product.Interfaces;
using Payments.BLL.Messaging.Product.Services;
using Payments.BLL.Services;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

builder.Configuration.AddEnvironmentVariables();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddControllers();

builder.Services.AddSwaggerGen();


builder.Services.AddSingleton<IHostedService, ServiceBackground>();
builder.Services.AddScoped<IStripeProductService, StripeProductService>();
builder.Services.AddSingleton<IStripeProductSubscriber, StripeProductSubscriber>();

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<PriceService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();

