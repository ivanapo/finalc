using triakl.DTOs;
using triakl.Models;

namespace triakl.Interface
{
    public interface IRoomRepository
    {
        ICollection<Room> GetAllRooms();
        ICollection<Guest> GetGuestFromARoom(int  roomId);
        Room GetRoomOfGuest(int roomId);
        Room GetRoomById(int id);
        bool CreateRoom(Room room);
        bool UpdateRoom(Room room);
        bool DeleteRoom(int id);
    }
}
