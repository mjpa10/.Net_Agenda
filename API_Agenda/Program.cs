using API_Agenda.DTOs.Mappings;
using API_Agenda.Logging;
using API_Agenda.Repository;
using API_Agenda.Services;
using APIAgenda.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "APIAgenda",
        Description = "Agenda de Contatos",
        Contact = new OpenApiContact
        {
            Name = "Matheus José",
            Email = "m.theus.jose.pereira@gmail.com",
            Url = new Uri("https://www.linkedin.com/in/matheus-josee/")
        }
    });

    var xmlFileName = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFileName));
});

builder.Services.AddScoped<IValidaEmail, ValidaEmail>();
builder.Services.AddScoped<IValidaTelefone, ValidaTelefone>();
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
builder.Services.AddScoped<IValidadorContato, ValidadorContato>();
builder.Services.AddScoped<IUnitOfwork, UnitOfWork>();
builder.Services.AddAutoMapper(typeof(ContatoDTOMappingProfile)); 

builder.Logging.AddProvider(new CustomLoggerProvider(new CustomLoggerProviderConfiguration
{ LogLevel = LogLevel.Information }));

string? mySqlConnection = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(mySqlConnection
    ,ServerVersion.AutoDetect(mySqlConnection)));   

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
