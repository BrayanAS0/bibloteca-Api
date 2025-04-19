using AutoMapper;
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
   public ComentarioController(ApplicationDbContext context, IMapper mapper)
   {
      _context = context;
      _mapper = mapper;
   }

   [HttpGet]
   public async Task<ActionResult<List<ComentarioDto>>> get(int libroId)
   {
      var comentarios = await _context.Comentarios.Include(x => x.Libros).Where(x => x.LibroId == libroId).ToListAsync();
      if (comentarios is not null)
         return _mapper.Map<List<ComentarioDto>>(comentarios);
      else
         return NoContent();
   }
   [HttpGet("{id}", Name = "ObtenerComnetario")]
   public async Task<ActionResult<ComentarioDto>> getById(Guid id)
   {
      var comentario = await _context.Comentarios.
      FirstOrDefaultAsync(x => x.Id == id);

      if (comentario is null)
         return NoContent();
      else
         return _mapper.Map<ComentarioDto>(comentario);
   }
   [HttpPost]
   public async Task<IActionResult> save(int libroId, ComentarioCreacionDto cometarioCreacion)
   {
      var comentario = await _context.Libros.
      FirstOrDefaultAsync(x => x.id == libroId);
      if (comentario is null)
         return NoContent();
      else
      {
         var comentarioC = _mapper.Map<Comentario>(cometarioCreacion);
         comentarioC.FechaDePublicacion = DateTime.Now;
         comentarioC.LibroId = libroId;
         _context.Comentarios.Add(comentarioC);
         await _context.SaveChangesAsync();
         return NoContent();
      }
   }

}