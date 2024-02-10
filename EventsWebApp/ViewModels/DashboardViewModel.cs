using EventsWebApp.Models;

namespace EventsWebApp.ViewModels
{
    public class DashboardViewModel
    {
        public List<Event> Events { get; set; }
        public List<Club> Clubs { get; set; }
    }
}
