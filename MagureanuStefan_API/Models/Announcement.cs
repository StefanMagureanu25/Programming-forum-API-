using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MagureanuStefan_API.Models
{
    public class Announcement
    {
        [Key]
        [JsonIgnore]
        public Guid IdAnnouncement { get; set; }

        //[Required(ErrorMessage = "Acest camp este obligatoriu!")]
        public DateTime? ValidFrom { get; set; }

        //[Required(ErrorMessage = "Acest camp este obligatoriu!")]
        public DateTime? ValidTo { get; set; }

        //[Required(ErrorMessage = "Acest camp este obligatoriu!")]
        //[StringLength(250, MinimumLength = 5, ErrorMessage = "Campul titlu poate sa contina maxim 250 caractere!!!!")]
        public string? Title { get; set; }

        //[Required(ErrorMessage = "Acest camp este obligatoriu!")]
        //[StringLength(1000, MinimumLength = 5, ErrorMessage = "Campul text poate sa contina maxim 1000 caractere!!!!")]
        public string? Text { get; set; }

        //[Required(ErrorMessage = "Acest camp este obligatoriu!")]
        public DateTime? EventDate { get; set; }

        //[Required(ErrorMessage = "Acest camp este obligatoriu!")]
        public string? Tags { get; set; }
    }
}
