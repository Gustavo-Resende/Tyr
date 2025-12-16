using Microsoft.EntityFrameworkCore;
using Tyr.Endpoints;
using Tyr.Infrastructure.Persistence;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Linq;


var builder = WebApplication.CreateBuilder(args);

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Tyr API", Version = "v1" });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) c.IncludeXmlComments(xmlPath);
});

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));
// register generic repository implementations
builder.Services.AddScoped(typeof(Tyr.Domain.Interfaces.IRepository<>), typeof(EfRepository<>));
builder.Services.AddScoped(typeof(Tyr.Domain.Interfaces.IReadRepository<>), typeof(EfRepository<>));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssemblyContaining<AssemblyMarker>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Tyr API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.MapServiceEndpoints();
app.MapCustomerEndpoints();
app.MapAppointmentEndpoints();
app.MapBusinessHourEndpoints();

// Start the app and, in development, open the Swagger UI in the default browser
await app.StartAsync();

if (app.Environment.IsDevelopment())
{
    try
    {
        var address = app.Urls.FirstOrDefault() ?? builder.Configuration["urls"] ?? "http://localhost:5000";
        var swaggerUrl = address.TrimEnd('/') + "/swagger";
        var psi = new ProcessStartInfo { FileName = swaggerUrl, UseShellExecute = true };
        Process.Start(psi);
    }
    catch { }
}

await app.WaitForShutdownAsync();
