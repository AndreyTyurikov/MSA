using InventoryMS.Host.Domain.DataLayer;
using InventoryMS.Host.MessageBroker;
using InventoryMS.Host.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options => {
    //Turn off camel-case JSON properties transformation
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Scoped services
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IMessageBusProducer, Producer>();
builder.Services.AddScoped<IEventService, EventService>();

//Transient
builder.Services.AddTransient<IInventoryDataLayer, InventoryDataLayer>();

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
