using triakl.DTOs;
using triakl.Models;

namespace triakl.Interface
{
    public interface IGuestRepository
    {
        ICollection<Guest> GetAllGuests();
        Guest GetGuestById(int id);
        bool GuestExists(int id);
        bool CreateGuest(Guest guest);
        bool UpdateGuest(Guest guest);
        bool DeleteGuest(int id);
    }
}
