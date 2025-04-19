using System.Text.Json.Serialization;
using biblotecaApi.Datos;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<ApplicationDbContext>(opciones=> opciones.UseSqlServer("name=DefaultConnection"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options => 
options.AddPolicy("AllowFronted",
policy => {
    policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
}));
var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
