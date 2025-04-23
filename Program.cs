using System.Text;
using biblotecaApi.Datos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// 🔐 Validar JWT desde config
var claveJwt = builder.Configuration["llavejwt"];
if (string.IsNullOrEmpty(claveJwt))
    throw new Exception("❌ No se encontró la clave JWT (llavejwt) en appsettings.json");

// 🔌 Validar cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    throw new Exception("❌ No se encontró 'DefaultConnection' en appsettings.json");

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddAutoMapper(typeof(Program));

// 💾 DB Context
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer(connectionString));

// 🔐 Identity
builder.Services.AddIdentityCore<IdentityUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<SignInManager<IdentityUser>>();
builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddHttpContextAccessor();

// 🔑 JWT
builder.Services.AddAuthentication().AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false;
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(claveJwt)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFronted", policy =>
    {
        policy.WithOrigins("http://localhost:3000").AllowAnyHeader().AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 🌐 Middlewares
app.UseCors("AllowFronted");

app.UseAuthentication();  // ← IMPORTANTE
app.UseAuthorization();

app.UseSwagger();
app.UseSwaggerUI();

// 🧠 Middleware para capturar errores
app.Use(async (context, next) =>
{
    try
    {
        await next();
    }
    catch (Exception ex)
    {
        Console.WriteLine($"💥 Excepción en tiempo de ejecución: {ex.Message}");
        throw;
    }
});

app.MapControllers();

try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error crítico al iniciar la app: {ex.Message}");
}
