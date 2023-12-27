using System.Text.Json;
using UserMS.Cache;
using UserMS.Domain.DataLayer;
using UserMS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options => {
        //Remove default JSON props naming policy (CamelCase)
        //This will keep mames as is
        options.JsonSerializerOptions.PropertyNamingPolicy = null;
    });
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Stateless services -> Transient
builder.Services.AddTransient<IPasswordService, PasswordService>();
builder.Services.AddTransient<IUserDataLayer, UserDataLayer>();

//Stateful services -> Scoped
builder.Services.AddScoped<IUserCacheClient, UserCacheClient>();
builder.Services.AddScoped<IUserService, UserService>();

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
