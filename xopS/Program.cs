using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using xopS;
using xopS.Controllers;
using xopS.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IDeviceService, DeviceService>();
builder.Services.AddSignalR();
    
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    DeviceService ds = new DeviceService();
    for (int i = 0; i < 5; i++)
    {
        ds.Add(new Device(
                        i.ToString(),
                        (i+1).ToString(),
                        i.ToString(),
                        i.ToString(),
                        i
        ));
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<DeviceHub>("/devicehub");

app.Run();