using EventsWebApp.Models;

namespace EventsWebApp.Interfaces
{
    public interface IEventRepository
    {
        Task<IEnumerable<Event>> GetAll();
        Task<Event> GetByIdAsync(int id);
        Task<Event> GetByIdAsyncNoTracks(int id);
        Task<IEnumerable<Event>> GetEventByCity(string city);
        bool Add(Event uevent);
        bool Update(Event uevent);
        bool Delete(Event uevent);
        bool Save();
    }
}
