using Microsoft.EntityFrameworkCore;
using triakl.Data;
using triakl.DTOs;
using triakl.Interface;
using triakl.Models;

namespace triakl.Repositories
{
    public class GuestRepository : IGuestRepository
    {
        private readonly DataContext _context;

        public GuestRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Guest> GetAllGuests()
        {
            return _context.Guest.OrderBy(p => p.Id).ToList();
        }

        public Guest GetGuestById(int id)
        {
            return _context.Guest.Where(p => p.Id == id)
                .Include(p => p.Room)
                .FirstOrDefault();
        }

        public bool GuestExists(int id)
        {
            return _context.Guest.Any(p => p.Id == id);
        }

        public bool CreateGuest(Guest guest)
        {
            _context.Add(guest);
            return Save();
        }

        public bool UpdateGuest(Guest guest)
        {
            var existingGuest = _context.Guest.Find(guest.Id);
            if (existingGuest != null)
            {
                _context.Entry(existingGuest).CurrentValues.SetValues(guest);
                return Save();
            }
            return false;
        }

        public bool DeleteGuest(int id)
        {
            var guest = _context.Guest.FirstOrDefault(p => p.Id == id);
            if (guest == null)
            {
                return false;
            }

            _context.Guest.Remove(guest);
            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
