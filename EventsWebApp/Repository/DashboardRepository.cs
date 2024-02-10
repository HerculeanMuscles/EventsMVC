using EventsWebApp.Data;
using EventsWebApp.Interfaces;
using EventsWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace EventsWebApp.Repository
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DashboardRepository(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<List<Club>> GetAllUserClubs()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userClubs = _context.Clubs.Where(r => r.AppUser.Id == curUser);
            return userClubs.ToList();
        }

        public async Task<List<Event>> GetAllUserEvents()
        {
            var curUser = _httpContextAccessor.HttpContext?.User.GetUserId();
            var userEvents = _context.Events.Where(r => r.AppUser.Id == curUser);
            return userEvents.ToList();
        }

        public async Task<AppUser> GetByIdNoTracking(string id)
        {
            return await _context.Users.Where(i => i.Id == id).AsNoTracking().FirstOrDefaultAsync();

        }

        public async Task<AppUser> GetUserById(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public bool Update(AppUser user)
        {
            _context.Users.Update(user);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
