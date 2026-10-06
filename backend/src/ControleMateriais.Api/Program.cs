using System.Text.Json.Serialization;
using ControleMateriais.Api.TratamentoErros;
using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.Servicos;
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

builder.Services.AddScoped<ServicoUnidade>();
builder.Services.AddScoped<ServicoMaterial>();
builder.Services.AddScoped<ServicoMovimentacaoEstoque>();
builder.Services.AddScoped<ServicoEstoque>();

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter()
        );
    });

builder.Services.AddOpenApi();

builder.Services.AddProblemDetails();

builder.Services.AddExceptionHandler<ApiExceptionHandler>();

var app = builder.Build();

app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
