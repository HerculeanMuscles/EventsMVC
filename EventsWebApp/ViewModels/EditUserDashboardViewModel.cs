using EventsWebApp.Models;

namespace EventsWebApp.ViewModels
{
    public class EditUserDashboardViewModel
    {
        public string Id { get; set; }
        public string? Description { get; set; }
        public string? ProfileImageUrl { get; set; }
        public IFormFile Image { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Street { get; set; }
    }
}
