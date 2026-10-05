using ControleMateriais.Application.Contratos;
using ControleMateriais.Infrastructure.Persistencia;
using ControleMateriais.Infrastructure.Persistencia.Repositorios;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("ControleMateriais")
    ?? throw new InvalidOperationException(
        "A connection string 'ControleMateriais' não foi configurada."
    );

builder.Services.AddDbContext<ControleMateriaisDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddScoped<IRepositorioUnidade, RepositorioUnidade>();

builder.Services.AddScoped<IRepositorioMaterial, RepositorioMaterial>();

builder.Services.AddScoped<
    IRepositorioMovimentacaoEstoque,
    RepositorioMovimentacaoEstoque
>();

builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
