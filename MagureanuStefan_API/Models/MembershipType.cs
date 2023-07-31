using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MagureanuStefan_API.Models
{
    public class MembershipType
    {
        [Key]
        [JsonIgnore]
        public Guid IdMembershipType { get; set; }

        public string? Name { get; set; }

        public string? Description { get; set; }

        public int? SubscriptionLengthInMonths { get; set; }
    }
}
