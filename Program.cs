using System.Text;
using bibloteca_api.Servicios;
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
    .AddRoles<IdentityRole>()//permite usar lo de roles
    .AddEntityFrameworkStores<ApplicationDbContext>()//para que las tablas de sql puedan cominacarse con indetity
    .AddDefaultTokenProviders();

builder.Services.AddScoped<SignInManager<IdentityUser>>();//permite autenticar usuarios
builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddTransient<IServiciosUsuarios, ServiciosUsuarios>();



builder.Services.AddHttpContextAccessor();//permite acceder al contexto en cuaquier clase

// 🔑 JWT
builder.Services.AddAuthentication().AddJwtBearer(opciones =>
{
    opciones.MapInboundClaims = false;// no me cambie el valor del claim automatico 
    opciones.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,//de lo mas importante
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
