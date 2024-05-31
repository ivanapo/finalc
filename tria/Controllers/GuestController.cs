using Microsoft.AspNetCore.Mvc;
using triakl.DTOs;
using triakl.Interface;
using triakl.Models;

namespace triakl.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly IGuestRepository _guestRepository;
        private readonly IRoomRepository _roomRepository;

        public GuestController(IGuestRepository guestRepository, IRoomRepository roomRepository)
        {
            _guestRepository = guestRepository;
            _roomRepository = roomRepository;
        }

        // GET: api/guest
        [HttpGet]
        public IActionResult GetAllGuests()
        {
            var guests = _guestRepository.GetAllGuests();
            return Ok(guests);
        }

        // GET: api/guest/{id}
        [HttpGet("{id}")]
        public IActionResult GetGuestById(int id)
        {
            var guest = _guestRepository.GetGuestById(id);
            if (guest == null)
            {
                return NotFound();
            }
            return Ok(guest);
        }

        // POST: api/guest
        [HttpPost]
        public IActionResult CreateGuest([FromBody] GuestDTO guestDto)
        {
            if (guestDto == null)
            {
                return BadRequest();
            }

            var room = _roomRepository.GetRoomById(guestDto.RoomId);
            if (room == null)
            {
                return BadRequest("Invalid Room ID");
            }

            var guest = new Guest
            {
                FirstName = guestDto.FirstName,
                LastName = guestDto.LastName,
                DOB = guestDto.DOB,
                Address = guestDto.Address,
                Nationality = guestDto.Nationality,
                CheckInDate = guestDto.CheckInDate,
                CheckOutDate = guestDto.CheckOutDate,
                Room = room
            };

            if (!_guestRepository.CreateGuest(guest))
            {
                ModelState.AddModelError("", "Something went wrong while creating the guest");
                return StatusCode(500, ModelState);
            }

            return CreatedAtAction("GetGuestById", new { id = guest.Id }, guest);
        }

        // PUT: api/guest/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateGuest(int id, [FromBody] GuestDTO guestDto)
        {
            if (guestDto == null)
            {
                return BadRequest();
            }

            var guest = _guestRepository.GetGuestById(id);
            if (guest == null)
            {
                return NotFound();
            }

            var room = _roomRepository.GetRoomById(guestDto.RoomId);
            if (room == null)
            {
                return BadRequest("Invalid Room ID");
            }

            guest.FirstName = guestDto.FirstName;
            guest.LastName = guestDto.LastName;
            guest.DOB = guestDto.DOB;
            guest.Address = guestDto.Address;
            guest.Nationality = guestDto.Nationality;
            guest.CheckInDate = guestDto.CheckInDate;
            guest.CheckOutDate = guestDto.CheckOutDate;
            guest.Room = room;

            if (!_guestRepository.UpdateGuest(guest))
            {
                ModelState.AddModelError("", "Something went wrong while updating the guest");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        // DELETE: api/guest/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteGuest(int id)
        {
            if (!_guestRepository.DeleteGuest(id))
            {
                ModelState.AddModelError("", "Something went wrong while deleting the guest");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
