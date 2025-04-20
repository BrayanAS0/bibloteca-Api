using System.Text;
using System.Text.Json.Serialization;
using biblotecaApi.Datos;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

builder.Services.AddIdentityCore<IdentityUser>().
AddEntityFrameworkStores<ApplicationDbContext>().
AddDefaultTokenProviders();

builder.Services.AddScoped<SignInManager<IdentityUser>>();

builder.Services.AddScoped<UserManager<IdentityUser>>();
builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication().AddJwtBearer(opciones =>{
opciones.MapInboundClaims =false;
opciones.TokenValidationParameters= new TokenValidationParameters{
    ValidateIssuer =false,
    ValidateAudience=false,
    ValidateLifetime =true,
    ValidateIssuerSigningKey =true,
    IssuerSigningKey  =
     new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["llavejwt"]!)),
     ClockSkew =TimeSpan.Zero
};
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();
app.MapControllers();

app.Run();
