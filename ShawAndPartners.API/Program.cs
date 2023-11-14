using Microsoft.OpenApi.Models;
using ShawAndPartners.API;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Inicializa o banco de dados
SQLitePCL.Batteries_V2.Init();
ServiceExtensions.InitializeDatabase(builder.Configuration);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddInjections(builder.Configuration);

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Challenge - Shaw And Partners", Version = "v1" });
    c.IncludeXmlComments(xmlPath);
});




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
