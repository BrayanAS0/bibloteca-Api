using biblotecaApi.Entidades;

namespace biblotecaApi.DTOS;
public class LibroConAutor:LibroDto{

public int AutorId {get;set;}
public required string AutorNombre {get;set;}
}