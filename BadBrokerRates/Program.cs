using BadBrokerRates.Service;
using Microsoft.EntityFrameworkCore;
using BadBrokerRates.Repository;
using ExchangeRates.Client;
using OpenExchangeRates.Client;
using BadBrokerRates.Services;
using BadBrokerRates.Providers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IRateService, RateService>();
builder.Services.AddScoped<IRateService, RateService>();
builder.Services.AddScoped<IRatesProvider, RatesProvider>();
builder.Services.AddScoped<IExchangeRatesClient, ExchangeRatesClient>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<BadBrokerRatesRepositoryContext>(options => options.UseSqlite($"Data Source={System.IO.Path.Join(Environment.CurrentDirectory, "BadBrokerRates.db")}" ));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseDefaultFiles();
    app.UseStaticFiles();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
