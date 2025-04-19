using AutoMapper;
using biblotecaApi.Datos;
using biblotecaApi.DTOS;
using biblotecaApi.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace biblotecaApi.Controllers;


[ApiController]
[Route("api/libros")]
public class LibrosController : ControllerBase
{
    private readonly ApplicationDbContext context;
private readonly IMapper _mapper;
    public LibrosController(ApplicationDbContext context,IMapper mapper)
    {
        _mapper=mapper;
        this.context = context;
    }
    [HttpGet("get")]
    public async Task<IEnumerable<LibroDto>> getLibros()
    {

        var libros = await context.Libros.ToListAsync();
        return _mapper.Map<IEnumerable<LibroDto>>(libros);
    }
    [HttpPost("post")]

    public async Task<ActionResult> CreateLibro(LibroCreacionDto libroCreacion)
    {
        if(libroCreacion is null || libroCreacion.AutoresIds.Count()==0){
             ModelState.AddModelError(nameof(libroCreacion.AutoresIds),"no se puede crear un libro sin autores");
             return ValidationProblem();
        }

var autoresIdsExisten= await context.Autores.Where(x => libroCreacion.AutoresIds.Contains(x.id)).Select(x => x.id).ToListAsync();
if(autoresIdsExisten.Count != libroCreacion.AutoresIds.Count){
    var autoresNoExisten = libroCreacion.AutoresIds.Except(autoresIdsExisten);
    var autoresNoExistenString =string.Join(",",autoresNoExisten);
    var mesanjeDeError=$"Los siguientes autores no existen : {autoresNoExistenString}";
    ModelState.AddModelError(nameof(libroCreacion.AutoresIds),mesanjeDeError);
return ValidationProblem();
}
 
        var libro = _mapper.Map<Libro>(libroCreacion);
 AsignarOrdenAutores(libro);
        context.Libros.Add(libro);
        await context.SaveChangesAsync();
        var libroDTO= _mapper.Map<LibroDto>(libro);
        return CreatedAtRoute("ObtenerLibros", new { id = libro.id }, libroDTO);
    }

private void AsignarOrdenAutores(Libro libro){
    if(libro.Autores is not null ){
        for(int  i =0;i<libro.Autores.Count;i++){
            libro.Autores[i].Orden=i;
        }
    }
}




    [HttpPut("put/{id:int}")]
    public async Task<ActionResult> updateLibro(int id, LibroCreacionDto libroCreacionDto)
    {
              if (libroCreacionDto is null || libroCreacionDto.AutoresIds.Count() == 0)
        {
            ModelState.AddModelError(nameof(libroCreacionDto.AutoresIds), "no se puede crear un libro sin autores");
            return ValidationProblem();
        }

        var autoresIdsExisten = await context.Autores.Where(x => libroCreacionDto.AutoresIds.Contains(x.id)).Select(x => x.id).ToListAsync();
        if (autoresIdsExisten.Count != libroCreacionDto.AutoresIds.Count)
        {
            var autoresNoExisten = libroCreacionDto.AutoresIds.Except(autoresIdsExisten);
            var autoresNoExistenString = string.Join(",", autoresNoExisten);
            var mesanjeDeError = $"Los siguientes autores no existen : {autoresNoExistenString}";
            ModelState.AddModelError(nameof(libroCreacionDto.AutoresIds), mesanjeDeError);
            return ValidationProblem();
        }

            var libro =_mapper.Map<Libro>(libroCreacionDto);
        context.Libros.Update(libro);
        await context.SaveChangesAsync();
        return NoContent();

    }
    // [HttpDelete("delete/{id:int}")]
    // public async Task<ActionResult> deleteLibro(int id)
    // {
    //     var regsitroBorrdados = await context.Libros.Where(item => item.id == id).ExecuteDeleteAsync();
    //     if (regsitroBorrdados != 0)
    //         return NotFound("no se borro nada");

    //     return NoContent();

    // }
    [HttpGet("{id:int}", Name = "ObtenerLibros")]
    public  ActionResult<LibroConAutor> getById(int id)
    {
        var libro = context.Libros.
       // Include(x => x.Autores).
        FirstOrDefault(x => x.id == id);
        if (libro is null)
            return NotFound();
        else
        return _mapper.Map<LibroConAutor>(libro);
    }


}