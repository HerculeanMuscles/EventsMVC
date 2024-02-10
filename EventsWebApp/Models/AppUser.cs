using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventsWebApp.Models
{
    public class AppUser : IdentityUser
    {
        [Key]
        [ForeignKey("Address")]
        public int? AddressId { get; set; }
        public Address? Address { get; set; }
        public string? Description { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Street { get; set; }
        public ICollection<Event> Events { get; set; }
        public ICollection<Club> Clubs { get; set; }
    }
}
