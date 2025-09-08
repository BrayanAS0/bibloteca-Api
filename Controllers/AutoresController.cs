using biblotecaApi.DTOS;
using biblotecaApi.Datos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using biblotecaApi.Entidades;
using Azure;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;

namespace biblotecaApi.Controllers;
[ApiController]
[Route("api/autores")]
[Authorize(Policy ="esadmin")]

public class AutoresController : ControllerBase
{
    private readonly ApplicationDbContext context;
    private readonly IMapper mapper;

    public AutoresController(ApplicationDbContext context,IMapper mapper)
    {
        this.context = context;
        this.mapper=mapper;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IEnumerable<AutorDto>> get()
    {
       var autores =await context.Autores.Include(x => x.Libros).ToListAsync();
       var AutorDto=mapper.Map<IEnumerable<AutorDto>>(autores);
       return AutorDto;
    }

    [HttpGet("{id:int}", Name = "ObtenerAutor")]
    [EndpointSummary("Para obtener autor a partir de un ID")]
    public async Task<ActionResult<AutorConLibrosDto>> GetById( int id)
    {
        var autor = await context.Autores.Include(x => x.Libros).FirstOrDefaultAsync(x => x.id == id);
        if (autor is null)
        {
            return NotFound();
        }
return mapper.Map<AutorConLibrosDto>(autor);
       
          }


    [HttpPost]
    public async Task<ActionResult> Post(AutorCreactionDto autoCreacion)
    {
        var autor = mapper.Map<Autor>(autoCreacion);
       context.Autores.Add(autor);
        await context.SaveChangesAsync();
       var AutorDto= mapper.Map<AutorDto>(autor);
               return CreatedAtRoute("ObtenerAutor",new {id=autor.id},AutorDto);
    }


[HttpPut("id:int")]
public async Task<ActionResult> Put(int id,AutorCreactionDto autorCreacionDto){
var autor = mapper.Map<Autor>(autorCreacionDto);
autor.id=id;
context.Update(autor);
await context.SaveChangesAsync();
return NoContent();
}

// [HttpPatch]

// public async Task<ActionResult> Patch(int id,JsonPatchDocument<AutorPatchDto> patchDoc ){

// }
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> delete(int id)
    {
        await context.Autores.Where(x => x.id == id).ExecuteDeleteAsync();
        return NoContent();
    }



}