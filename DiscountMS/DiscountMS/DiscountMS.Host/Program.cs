using DiscountMS.Host.Domain.DataLayer;
using DiscountMS.Host.Domain.DbCtx;
using DiscountMS.Host.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Scoped (1 instance per 1 HTTP request)
builder.Services.AddScoped<IDiscountService, DiscountService>();

//Transient (1 instance per 1 new injection)
builder.Services.AddTransient<IDiscountDataLayer, DiscountDataLayer>();

//Singlentone (1 instance ever)

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();
