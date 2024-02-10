using EventsWebApp.Models;

namespace EventsWebApp.Interfaces
{
    public interface IDashboardRepository
    {
        Task<List<Event>> GetAllUserEvents();
        Task<List<Club>> GetAllUserClubs();
        Task<AppUser> GetUserById(string id);
        Task<AppUser> GetByIdNoTracking(string id);
        bool Update(AppUser user);
        bool Save();
    }
}
