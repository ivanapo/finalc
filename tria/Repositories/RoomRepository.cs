using System.Diagnostics.Metrics;
using triakl.Data;
using triakl.DTOs;
using triakl.Interface;
using triakl.Models;

namespace triakl.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly DataContext _context;

        public RoomRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Room> GetAllRooms()
        {
           return _context.Room.ToList();
        }

        public Room GetRoomById(int id)
        {
            return _context.Room.Where(e => e.RoomId == id).FirstOrDefault();
        }

        public ICollection<Guest> GetGuestFromARoom(int roomId)
        {
            return _context.Guest.Where(c => c.Room.RoomId == roomId).ToList();
        }

        public Room GetRoomOfGuest(int id)
        {
            return _context.Guest.Where(o => o.Id == id).Select(c => c.Room).FirstOrDefault();
        }
        public bool CreateRoom(Room room)
        {
                _context.Add(room);
                return Save();
        }

        public bool UpdateRoom(Room room)
        {
            var existingroom = _context.Room.Find(room.RoomId);
            if (existingroom == null)
            {
                return false;
            }

            _context.Entry(existingroom).CurrentValues.SetValues(room);
            return Save();
        }

        public bool DeleteRoom(int id)
        {
            var room = _context.Room.FirstOrDefault(p => p.RoomId == id);
            if (room == null)
            {
                return false;
            }

            _context.Room.Remove(room);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
