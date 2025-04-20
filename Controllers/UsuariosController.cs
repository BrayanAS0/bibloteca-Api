using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.CompilerServices;
using System.Security.Claims;
using biblotecaApi.Datos;
using biblotecaApi.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace biblotecaApi.Controllers;
[ApiController]
[Route("api/Usuarios")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private ApplicationDbContext _contex;
    private readonly IConfiguration _configuration;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;

    public UsuariosController(ApplicationDbContext context,
     UserManager<IdentityUser> userManager,
      IConfiguration configuration,
      SignInManager<IdentityUser> signInManager
      )
    {
        _contex = context;
        _configuration = configuration;
        _userManager = userManager;
            _signInManager = signInManager;

    }
    [HttpPost("registro")]
    [AllowAnonymous]
    [Description("you need access to use it")]
    public async Task<ActionResult<RespuestaAutenticacionDTO>> Registrar(CredencialesDTO credencialesUsuariosDTO)
    {
        var usuario = new IdentityUser
        {
            UserName = credencialesUsuariosDTO.Email,
            Email = credencialesUsuariosDTO.Email

        };
        var resultado = await _userManager.CreateAsync(usuario, credencialesUsuariosDTO.Password!);
        if (resultado.Succeeded)
        {
            var respuestaAutenticacion = await contruirToken(credencialesUsuariosDTO);
            return respuestaAutenticacion;
        }
        else
        {
            foreach (var error in resultado.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return ValidationProblem();
        }
    }
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<RespuestaAutenticacionDTO>> Login(CredencialesDTO credencialesDTO)
    {
        var usuario = await _userManager.FindByEmailAsync(credencialesDTO.Email);

        if (usuario is null)
        {
            return RetornarLogingIncorrecto();
        }
        


        var resultado = await _signInManager.CheckPasswordSignInAsync(usuario,
        credencialesDTO.Password!, lockoutOnFailure: false);
if(resultado.Succeeded){
    return await contruirToken(credencialesDTO);
}else{
    return RetornarLogingIncorrecto();
}
    }
    private ActionResult RetornarLogingIncorrecto()
    {
        ModelState.AddModelError(string.Empty, "Loging Incorrecto");
        return ValidationProblem();
    }
    private async Task<RespuestaAutenticacionDTO> contruirToken(CredencialesDTO credencialesDTO)
    {
        var claims = new List<Claim>{
            new Claim("email",credencialesDTO.Email),
            new Claim("lo que yo quiera ","cualquie valor")
        };
        var usuario = await _userManager.FindByEmailAsync(credencialesDTO.Email);
        var claimsDB = await _userManager.GetClaimsAsync(usuario!);
        claims.AddRange(claimsDB);
        var llave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration["llavejwt"]!));
        var credenciales = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
        var expiracion = DateTime.UtcNow.AddYears(1);
        var tokenSeguridad = new JwtSecurityToken(issuer: null, audience: null, claims, expires: expiracion, signingCredentials: credenciales);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenSeguridad);
        return new RespuestaAutenticacionDTO
        {
            Token = token,
            Expiracion = expiracion
        };
    }
}