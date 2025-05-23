using biblotecaApi.Entidades;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace biblotecaApi.Datos;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    { }

    public DbSet<Autor> Autores { get; set; }
    public DbSet<Libro> Libros { set; get; }
    public DbSet<Comentario> Comentarios { get; set; }
    public DbSet<AutorLibro> AutoresLibros { get; set; }
}