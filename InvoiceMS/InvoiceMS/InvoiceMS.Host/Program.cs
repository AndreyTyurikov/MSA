using InventoryMS.Client;
using InvoiceMS.Infrastructure;
using InvoiceMS.Infrastructure.DataLayer;
using InvoiceMS.Infrastructure.EventProcessors;
using InvoiceMS.Infrastructure.MessageBroker;
using InvoiceMS.Infrastructure.Services;
using UserMS.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Scoped services
builder.Services.AddScoped<IInvoiceService, InvoiceService>();

//Transient services
builder.Services.AddTransient<IInvoiceDataLayer, InvoiceDataLayer>();
builder.Services.AddTransient<IInvetoryServiceEventsProcessor, InvetoryServiceEventsProcessor>();
builder.Services.AddTransient<IInventoryUpdateNotificationsProcessor, InvoiceService>();

//Singleton services
builder.Services.AddSingleton<IUserMsClient, UserMsClient>();

//Hosted services
builder.Services.AddHostedService<InventoryServiceEventsConsumer>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
