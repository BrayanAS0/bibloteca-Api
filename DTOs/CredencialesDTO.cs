using System.ComponentModel.DataAnnotations;

namespace biblotecaApi.DTOS;

public class CredencialesDTO{
[Required]
[EmailAddress]
public required string Email {get;set;}
[Required]
public string? Password {set;get;}

}