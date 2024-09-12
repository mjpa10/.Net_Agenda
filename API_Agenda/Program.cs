using API_Agenda.Logging;
using API_Agenda.Repository;
using API_Agenda.Services;
using APIAgenda.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IValidaEmail, ValidaEmail>();
builder.Services.AddScoped<IValidaAniversario, ValidaAniversario>();
builder.Services.AddScoped<IValidaTelefone, ValidaTelefone>();
builder.Services.AddScoped<IContatoRepository, ContatoRepository>();
builder.Services.AddScoped<IValidadorContato, ValidadorContato>();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
