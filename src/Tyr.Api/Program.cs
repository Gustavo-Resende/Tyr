using Microsoft.EntityFrameworkCore;
using Tyr.Endpoints;
using Tyr.Infrastructure.Persistence;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<AssemblyMarker>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapServiceEndpoints();
app.MapCustomerEndpoints();
app.MapAppointmentEndpoints();
app.MapBusinessHourEndpoints();

app.Run();
