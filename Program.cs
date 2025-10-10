using Microsoft.EntityFrameworkCore;
using Tyr.Data;
using Tyr.Endpoints;
using Tyr.Models;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapServiceEndpoints();
app.MapProfessionalEndpoints();
app.MapClienteEndpoint();

app.Run();
