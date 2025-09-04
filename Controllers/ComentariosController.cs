using AutoMapper;
using Azure;
using bibloteca_api.DTOs;
using bibloteca_api.Servicios;
using biblotecaApi.Datos;
using biblotecaApi.DTOS;
using biblotecaApi.Entidades;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace biblotecaApi.Controllers;
[ApiController]
[Route("api/libros/{libroId:int}/comentarios")]
public class ComentarioController : ControllerBase
{

   private readonly ApplicationDbContext _context;
   private readonly IMapper _mapper;
    private readonly IServiciosUsuarios _serviciosUsuarios;
   public ComentarioController(ApplicationDbContext context, IMapper mapper,IServiciosUsuarios serviciosUsuarios)
   {
      _context = context;
      _mapper = mapper;
        _serviciosUsuarios = serviciosUsuarios;
   }

   [HttpGet]
   public async Task<ActionResult<List<ComentarioDto>>> Get(int libroId)
   {
var existeLibro =await  _context.Libros.AnyAsync(X=> X.id == libroId);
        if (!existeLibro) return NotFound();
        var comentarios =await  _context.Comentarios.Include(x=> x.Usuario).Where(x=> x.LibroId== libroId).ToListAsync();
   return _mapper.Map<List<ComentarioDto>>(comentarios);
    }
   [HttpGet("{id}", Name = "ObtenerComnetario")]
   public async Task<ActionResult<ComentarioDto>> getById(Guid id)
   {
      var comentario = await _context.Comentarios
            .Include(x=> x.Usuario).
            FirstOrDefaultAsync(x => x.Id == id);

      if (comentario is null)
         return NoContent();
      else
         return _mapper.Map<ComentarioDto>(comentario);
   }
   [HttpPost]
   public async Task<IActionResult> save(int libroId, ComentarioCreacionDto cometarioCreacion)
   {
        var existeLibro = await _context.Libros.AnyAsync(x=> x.id == libroId);
        if (!existeLibro) return NotFound();
        var usuario =  await _serviciosUsuarios.ObetenerUsuario();
        if(usuario == null)return NotFound();

        var comentario = _mapper.Map<Comentario>(cometarioCreacion);
        comentario.LibroId = libroId;
        comentario.FechaDePublicacion=DateTime.UtcNow;
        comentario.UsuarioId = usuario.Id;
        _context.Add(comentario);
        await _context.SaveChangesAsync();
       
        var cometarioDto = _mapper.Map<ComentarioDto>(comentario);
        return CreatedAtRoute("ObtenerComnetario", new {id=comentario.Id,libroId=libroId});

   }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> Pathc(Guid id, int libroId, JsonPatchDocument<ComentarioPatchDTO> pathcDoc)
    {
        if (pathcDoc is null) return BadRequest();
        var existenteLibro = await _context.Libros.AnyAsync(x => x.id == libroId);
        if (!existenteLibro) return BadRequest();


        var usuario = await _serviciosUsuarios.ObetenerUsuario();
        if (usuario == null) return NotFound();


        var comentarioDb = await _context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);
        if (comentarioDb is null) return NotFound();

        if (comentarioDb.UsuarioId != usuario.Id) return Forbid();

        var comentarioPathcDto = _mapper.Map<ComentarioPatchDTO>(comentarioDb);

        pathcDoc.ApplyTo(comentarioPathcDto,ModelState);
        var esValido = TryValidateModel(comentarioPathcDto);
        if (!esValido)
        {
            return ValidationProblem();
        }

        _mapper.Map(comentarioPathcDto, comentarioDb);

        await _context.SaveChangesAsync();

        return NoContent();
    }
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id, int libroId)
    {
        var existeLibro = await _context.Libros.AnyAsync(x => x.id == libroId);

        if (!existeLibro)
        {
            return NotFound();
        }

        var usuario = await _serviciosUsuarios.ObetenerUsuario();

        if (usuario is null)
        {
            return NotFound();
        }

        var comentarioDB = await _context.Comentarios.FirstOrDefaultAsync(x => x.Id == id);

        if (comentarioDB is null)
        {
            return NotFound();
        }

        if (comentarioDB.UsuarioId != usuario.Id)
        {
            return Forbid();
        }

        _context.Remove(comentarioDB);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}