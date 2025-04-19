using biblotecaApi.Datos;
using Microsoft.AspNetCore.Mvc;

namespace biblotecaApi;
[ApiController]
[Route("api/configuraciones")]
public class ConfiguracionesController:ControllerBase{
private ApplicationDbContext _context;
private IConfiguration _config;
public ConfiguracionesController(ApplicationDbContext context,IConfiguration config){
_config=config;
_context=context;
}

[HttpGet]
public ActionResult<string> get(){
var config = _config["ConnectionStrings:DefaultConnection"]!;
return config;

}
}