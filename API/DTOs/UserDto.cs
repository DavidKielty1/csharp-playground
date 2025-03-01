using System.ComponentModel.DataAnnotations;

namespace API.DTOs
{
    public class UserDto
    {
            public string? Username {get; set;}

            [Required] public string? KnownAs { get; set; }
            public string? Token {get; set;}
            public string? PhotoUrl { get; set; }
    }
}