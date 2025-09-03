using AutoMapper;
using bibloteca_api.Servicios;
using biblotecaApi.Datos;
using biblotecaApi.DTOS;
using biblotecaApi.Entidades;
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
        var usuario = _serviciosUsuarios.ObetenerUsuario();
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


}