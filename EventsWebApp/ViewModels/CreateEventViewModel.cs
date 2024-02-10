using EventsWebApp.Data.Enum;
using EventsWebApp.Models;

namespace EventsWebApp.ViewModels
{
    public class CreateEventViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Address Address { get; set; }
        public IFormFile Image { get; set; }
        public EventCategory EventCategory { get; set; }
        public string AppUserId { get; set; }

    }
}
