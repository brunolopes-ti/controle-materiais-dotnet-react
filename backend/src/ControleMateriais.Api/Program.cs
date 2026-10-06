using System.Text;
using System.Text.Json.Serialization;
using ControleMateriais.Api.Seguranca;
using ControleMateriais.Api.TratamentoErros;
using ControleMateriais.Application.Contratos;
using ControleMateriais.Application.Servicos;
using ControleMateriais.Infrastructure.Persistencia;
using ControleMateriais.Infrastructure.Persistencia.Repositorios;
using ControleMateriais.Infrastructure.Seguranca;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var connectionString =
    builder.Configuration.GetConnectionString("ControleMateriais")
    ?? throw new InvalidOperationException(
        "A connection string 'ControleMateriais' não foi configurada."
    );

var jwtKey =
    builder.Configuration["Jwt:Key"]
    ?? throw new InvalidOperationException(
        "A chave JWT não foi configurada."
    );

var jwtIssuer =
    builder.Configuration["Jwt:Issuer"]
    ?? throw new InvalidOperationException(
        "O emissor JWT não foi configurado."
    );

var jwtAudience =
    builder.Configuration["Jwt:Audience"]
    ?? throw new InvalidOperationException(
        "A audiência JWT não foi configurada."
    );

if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
{
    throw new InvalidOperationException(
        "A chave JWT deve possuir pelo menos 32 bytes."
    );
}

builder.Services.AddDbContext<ControleMateriaisDbContext>(options =>
    options.UseNpgsql(connectionString)
);

builder.Services.AddScoped<IRepositorioUnidade, RepositorioUnidade>();
builder.Services.AddScoped<IRepositorioMaterial, RepositorioMaterial>();
builder.Services.AddScoped<IRepositorioUsuario, RepositorioUsuario>();

builder.Services.AddScoped<
    IRepositorioMovimentacaoEstoque,
    RepositorioMovimentacaoEstoque
>();

builder.Services.AddScoped<IServicoHashSenha, ServicoHashSenha>();
builder.Services.AddScoped<IGeradorToken, GeradorTokenJwt>();

builder.Services.AddScoped<ServicoUnidade>();
builder.Services.AddScoped<ServicoMaterial>();
builder.Services.AddScoped<ServicoMovimentacaoEstoque>();
builder.Services.AddScoped<ServicoEstoque>();
builder.Services.AddScoped<ServicoHistoricoMovimentacao>();
builder.Services.AddScoped<ServicoResumoEstoque>();
builder.Services.AddScoped<ServicoUsuario>();
builder.Services.AddScoped<ServicoAutenticacao>();

builder.Services
    .AddAuthentication(
        JwtBearerDefaults.AuthenticationScheme
    )
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =
                    new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtKey)
                    ),

                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,

                ValidateAudience = true,
                ValidAudience = jwtAudience,

                ValidateLifetime = true,

                ClockSkew = TimeSpan.Zero
            };
    });

builder.Services.AddAuthorization();

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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
