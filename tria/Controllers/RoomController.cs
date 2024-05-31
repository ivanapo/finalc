using Microsoft.AspNetCore.Mvc;
using triakl.DTOs;
using triakl.Interface;
using triakl.Models;

namespace triakl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomRepository _roomRepository;

        public RoomController(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }

        // GET: api/room
        [HttpGet]
        public IActionResult GetAllRooms()
        {
            var rooms = _roomRepository.GetAllRooms();
            return Ok(rooms);
        }

        // GET: api/room/{id}
        [HttpGet("{id}")]
        public IActionResult GetRoomById(int id)
        {
            var room = _roomRepository.GetRoomById(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }

        // GET: api/room/{roomId}/guests
        [HttpGet("{roomId}/guests")]
        public IActionResult GetGuestsFromRoom(int roomId)
        {
            var guests = _roomRepository.GetGuestFromARoom(roomId);
            if (guests == null || guests.Count == 0)
            {
                return NotFound();
            }
            return Ok(guests);
        }

        // POST: api/room
        [HttpPost]
        public IActionResult CreateRoom([FromBody] RoomDTO roomDto)
        {
            if (roomDto == null)
            {
                return BadRequest();
            }

            var room = new Room
            {
                Number = roomDto.Number,
                Floor = roomDto.Floor,
                Type = roomDto.Type
            };

            if (!_roomRepository.CreateRoom(room))
            {
                ModelState.AddModelError("", "Something went wrong while creating the room");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction("GetRoomById", new { id = room.RoomId }, room);
        }

        // PUT: api/room/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateRoom(int id, [FromBody] RoomDTO roomDto)
        {
            if (roomDto == null)
            {
                return BadRequest();
            }

            var room = _roomRepository.GetRoomById(id);
            if (room == null)
            {
                return NotFound();
            }

            room.Number = roomDto.Number;
            room.Floor = roomDto.Floor;
            room.Type = roomDto.Type;

            if (!_roomRepository.UpdateRoom(room))
            {
                ModelState.AddModelError("", "Something went wrong while updating the room");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        // DELETE: api/room/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteRoom(int id)
        {
            if (!_roomRepository.DeleteRoom(id))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the room");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpGet("guest/{id}/room")]
        public IActionResult GetRoomOfGuest(int id)
        {
            var room = _roomRepository.GetRoomOfGuest(id);
            if (room == null)
            {
                return NotFound();
            }
            return Ok(room);
        }
    }
}
