using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MagureanuStefan_API.Models
{
    public class Member
    {
        [Key]
        [JsonIgnore]
        public Guid IdMember { get; set; }

        //[StringLength(250, MinimumLength = 5, ErrorMessage = "Campul name poate sa contina maxim 250 caractere!!!!")]
        public string? Name { get; set; }

        //[StringLength(100, MinimumLength = 5, ErrorMessage = "Campul title poate sa contina maxim 100 caractere!!!!")]
        public string? Title { get; set; }

        //[StringLength(250, MinimumLength = 5, ErrorMessage = "Campul position poate sa contina maxim 250 caractere!!!!")]
        public string? Position { get; set; }

        //[StringLength(1000, MinimumLength = 5, ErrorMessage = "Campul description poate sa contina maxim 1000 caractere!!!!")]
        public string? Description { get; set; }

        public string? Resume { get; set; }

        //[StringLength(100, MinimumLength = 5, ErrorMessage = "Campul username poate sa contina maxim 100 caractere!!!!")]
        public string? Username { get; set; }

        //[StringLength(100, MinimumLength = 5, ErrorMessage = "Campul password poate sa contina maxim 100 caractere!!!!")]
        public string? Password { get; set; }
    }
}
