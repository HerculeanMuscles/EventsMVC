using EventsWebApp.Data;
using EventsWebApp.Interfaces;
using EventsWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Repository
{
    public class EventRepository : IEventRepository
    {

        private readonly ApplicationDbContext _context;

        public EventRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public bool Add(Event uevent)
        {
            _context.Add(uevent);
            return Save();
        }

        public bool Delete(Event uevent)
        {
            _context.Remove(uevent);
            return Save();
        }

        public async Task<IEnumerable<Event>> GetAll()
        {
            return await _context.Events.ToListAsync();
        }

        public async Task<Event> GetByIdAsync(int id)
        {
            return await _context.Events.Include(i => i.Address).FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<Event> GetByIdAsyncNoTracks(int id)
        {
            return await _context.Events.Include(i => i.Address).AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Event>> GetEventByCity(string city)
        {
            return await _context.Events.Where(c => c.Address.City.Contains(city)).ToListAsync();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool Update(Event uevent)
        {
            _context.Update(uevent);
            return Save();
        }
    }
}
