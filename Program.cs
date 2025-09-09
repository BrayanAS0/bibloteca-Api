using bibloteca_api.Entidades;
using bibloteca_api.Servicios;
using bibloteca_api.Swagger;
using biblotecaApi.Datos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
builder.Services.AddIdentityCore<Usuario>()
    .AddRoles<IdentityRole>()//permite usar lo de roles
    .AddEntityFrameworkStores<ApplicationDbContext>()//para que las tablas de sql puedan cominacarse con indetity
    .AddDefaultTokenProviders();

builder.Services.AddScoped<SignInManager<Usuario>>();//permite autenticar usuarios
builder.Services.AddScoped<UserManager<Usuario>>();
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
builder.Services.AddSwaggerGen(c =>
{
    c.OperationFilter<AuthorizeCheckOperationFilter>();
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Mi API",
        Version = "v1"
    });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, incluye el JWT con Bearer en el campo",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });
    //c.AddSecurityRequirement(new OpenApiSecurityRequirement
    //{
    //    {
    //        new OpenApiSecurityScheme
    //        {
    //            Reference = new OpenApiReference
    //            {
    //                Type = ReferenceType.SecurityScheme,
    //                Id = "Bearer"
    //            }
    //        },
    //        new string[] {}
    //    }
    //});
});
builder.Services.AddAuthorization(opciones =>
{
    opciones.AddPolicy("esadmin", politica => politica.RequireClaim("esadmin"));
});



///cors 
///
var allowHost = builder.Configuration.GetSection("AllowedHosts").Get<string[]>()!;

builder.Services.AddCors(options => options.AddPolicy("AllowCors",
    builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins(allowHost)
    ));
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
app.UseCors("allowCors");
try
{
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Error crítico al iniciar la app: {ex.Message}");
}
