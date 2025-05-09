using Aplicacao.Aplicacoes;
using Aplicacao.Interfaces;
using AutoRegistro.Token;
using Dominio.Interfaces.Generico;
using Dominio.Interfaces;
using InfraEstrutura.Configuracao;
using InfraEstrutura.Repositorio.Generico;
using Infraestrutura.Repositorio;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Entidades.Entidades;
using Dominio.Servicos;
using Dominio.Interfaces.InterfacesServicos;
using InfraEstrutura.Repositorio;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<Contexto>(options =>
options.UseMySql(builder.Configuration.GetConnectionString("Default"),
   new MySqlServerVersion(new Version(10, 2, 36))));

builder.Services.AddDefaultIdentity<Usuario>(options =>
  options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<Contexto>();

builder.Services.AddSingleton(typeof(IGenerico<>), typeof(RepositorioGenerico<>));

builder.Services.AddScoped<IUsuario, RepositorioUsuario>();
builder.Services.AddScoped<IVeiculo, RepositorioVeiculo>();
builder.Services.AddScoped<IManutencao, RepositorioManutencao>();

builder.Services.AddScoped<IUsuarioServico, UsuarioServico>();
builder.Services.AddScoped<IVeiculoServico, VeiculoServico>();
builder.Services.AddScoped<IManutencaoServico, ManutencaoServico>();

builder.Services.AddScoped<IAplicacaoUsuario, AplicacaoUsuario>();
builder.Services.AddScoped<IAplicacaoVeiculo, AplicacaoVeiculo>();
builder.Services.AddScoped<IAplicacaoManutencao, AplicacaoManutencao>();
builder.Services.AddScoped<TokenJwtBuilder>();


//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("RequireAdministratorRole", policy =>
//    {
//        policy.RequireAuthenticatedUser();
//        policy.RequireClaim("tipo", "Administrador");
//    });
//});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(option =>
    {
        option.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = "Security.Bearer",
            ValidAudience = "Security.Bearer",

            IssuerSigningKey = JwtSecurityKey.Creater("Secret_Key-12345678")
        };

        option.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                return Task.CompletedTask;
            },
            OnTokenValidated = context =>
            {
                Console.WriteLine("OnTokenValidated: " + context.SecurityToken);
                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebApi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = @"JWT Authorization header usando o Bearer.
                        Entre com 'Bearer ' [espaço] então coloque seu token.
                        Exemplo: 'Bearer 12345oiuytr'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

var frontClient = "http://localhost:4200";
//var frontClient2 = "https://caioportifolio.azurewebsites.net/";
app.UseCors(x =>
x.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader()
.WithOrigins(frontClient));

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
