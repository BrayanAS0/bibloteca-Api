using AutoMapper;
using bibloteca_api.DTOs;
using bibloteca_api.Entidades;
using biblotecaApi.DTOS;
using biblotecaApi.Entidades;
using Microsoft.AspNetCore.Identity;
using System.IO.Compression;

namespace biblotecaApi.Utiliades;

public class AutoMapperProfiles : Profile
{
    public AutoMapperProfiles()
    {

        CreateMap<Autor, AutorDto>().
        ForMember(dto => dto.NombreCompleto,// a que propeida de destino le queor asignar un valor
        config => config.MapFrom(autor => $"{autor.Nombres} {autor.Apellidos}"));
        CreateMap<Autor, AutorConLibrosDto>().
ForMember(dto =>
    dto.NombreCompleto, config =>
        config.MapFrom(autor => $"{autor.Nombres} {autor.Apellidos}"));



        CreateMap<AutorCreactionDto, Autor>().ReverseMap();


        CreateMap<Libro, LibroDto>();

CreateMap<Libro,LibroConAutor>();
CreateMap<LibroCreacionDto,Libro>().
ForMember(ent => ent.Autores ,
config => config.MapFrom(dto =>
 dto.AutoresIds.Select (x=>
  new AutorLibro {AutorId =x}) ));





         
        // CreateMap<Libro, LibroConAutor>()
        //      .ForMember(dest => dest.AutorNombre,// aque propieda de destino le quiero asignar un valor
        //       opt => opt.MapFrom(src => $" {src.Autor!.Nombres} {src.Autor!.Apellidos}"));// cual propiedad de Libro quires asignar
       
       
        CreateMap<Comentario, ComentarioDto>();

        CreateMap<ComentarioCreacionDto,Comentario>();
        CreateMap<Comentario,ComentarioDto>().ForMember(dest=> dest.UsuarioEmail ,config=> config.MapFrom(ent => ent.Usuario!.Email));
        CreateMap<CredencialesDTO, bibloteca_api.Entidades.Usuario>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email)).ReverseMap();




        ///////////////////////
        ///

        CreateMap<UsuarioDto, Usuario>().ReverseMap();
    }
}

