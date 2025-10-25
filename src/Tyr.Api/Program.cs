using Microsoft.EntityFrameworkCore;
using Tyr.Domain.Interfaces;
using Tyr.Endpoints;
using Tyr.Infrastructure.Persistence;
using Tyr.Repository;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProfissionalRepository, ProfissionalRepository>();
builder.Services.AddScoped<IServicoRepository, ServicoRepository>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapServicoEndpoints();
app.MapProfissionalEndpoints();
app.MapClienteEndpoints();
app.MapAgendamentoEndpoints();

app.Run();
