using Microsoft.AspNetCore.Mvc;
using Moq;
using triakl.Controllers;
using triakl.DTOs;
using triakl.Interface;
using triakl.Models;


namespace Hotel.Test.ControllersTest
{
    public class GuestControllerTests
    {
        private readonly Mock<IGuestRepository> _mockGuestRepository;
        private readonly Mock<IRoomRepository> _mockRoomRepository;
        private readonly GuestController _controller;

        public GuestControllerTests()
        {
            _mockGuestRepository = new Mock<IGuestRepository>();
            _mockRoomRepository = new Mock<IRoomRepository>();
            _controller = new GuestController(_mockGuestRepository.Object, _mockRoomRepository.Object);
        }

        [Fact]
        public void GetAllGuests_ReturnsOkResult_WithListOfGuests()
        {
            // Arrange
            var guests = new List<Guest> { new Guest(), new Guest() };
            _mockGuestRepository.Setup(repo => repo.GetAllGuests()).Returns(guests);

            // Act
            var result = _controller.GetAllGuests();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnGuests = Assert.IsType<List<Guest>>(okResult.Value);
            Assert.Equal(2, returnGuests.Count);
        }

        [Fact]
        public void GetGuestById_ReturnsNotFound_WhenGuestDoesNotExist()
        {
            // Arrange
            _mockGuestRepository.Setup(repo => repo.GetGuestById(It.IsAny<int>())).Returns((Guest)null);

            // Act
            var result = _controller.GetGuestById(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetGuestById_ReturnsOkResult_WithGuest()
        {
            // Arrange
            var guest = new Guest { Id = 1 };
            _mockGuestRepository.Setup(repo => repo.GetGuestById(1)).Returns(guest);

            // Act
            var result = _controller.GetGuestById(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnGuest = Assert.IsType<Guest>(okResult.Value);
            Assert.Equal(1, returnGuest.Id);
        }

        [Fact]
        public void CreateGuest_ReturnsBadRequest_WhenGuestDtoIsNull()
        {
            // Act
            var result = _controller.CreateGuest(null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void CreateGuest_ReturnsBadRequest_WhenRoomDoesNotExist()
        {
            // Arrange
            var guestDto = new GuestDTO { RoomId = 1 };
            _mockRoomRepository.Setup(repo => repo.GetRoomById(1)).Returns((Room)null);

            // Act
            var result = _controller.CreateGuest(guestDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Room ID", badRequestResult.Value);
        }

        [Fact]
        public void CreateGuest_ReturnsCreatedAtActionResult_WhenGuestIsCreated()
        {
            // Arrange
            var guestDto = new GuestDTO
            {
                FirstName = "John",
                LastName = "Doe",
                DOB = DateTime.Now,
                Address = "123 Main St",
                Nationality = "USA",
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now,
                RoomId = 1
            };
            var room = new Room { RoomId = 1 };
            _mockRoomRepository.Setup(repo => repo.GetRoomById(1)).Returns(room);
            _mockGuestRepository.Setup(repo => repo.CreateGuest(It.IsAny<Guest>())).Returns(true);

            // Act
            var result = _controller.CreateGuest(guestDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var createdGuest = Assert.IsType<Guest>(createdAtActionResult.Value);
            Assert.Equal(guestDto.FirstName, createdGuest.FirstName);
            Assert.Equal(guestDto.LastName, createdGuest.LastName);
        }

        [Fact]
        public void UpdateGuest_ReturnsBadRequest_WhenGuestDtoIsNull()
        {
            // Act
            var result = _controller.UpdateGuest(1, null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void UpdateGuest_ReturnsNotFound_WhenGuestDoesNotExist()
        {
            // Arrange
            _mockGuestRepository.Setup(repo => repo.GetGuestById(It.IsAny<int>())).Returns((Guest)null);

            // Act
            var result = _controller.UpdateGuest(1, new GuestDTO());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateGuest_ReturnsBadRequest_WhenRoomDoesNotExist()
        {
            // Arrange
            var guestDto = new GuestDTO { RoomId = 1 };
            _mockGuestRepository.Setup(repo => repo.GetGuestById(1)).Returns(new Guest { Id = 1 });
            _mockRoomRepository.Setup(repo => repo.GetRoomById(1)).Returns((Room)null);

            // Act
            var result = _controller.UpdateGuest(1, guestDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Invalid Room ID", badRequestResult.Value);
        }

        [Fact]
        public void UpdateGuest_ReturnsNoContent_WhenGuestIsUpdated()
        {
            // Arrange
            var guestDto = new GuestDTO
            {
                FirstName = "Jane",
                LastName = "Doe",
                DOB = DateTime.Now,
                Address = "456 Main St",
                Nationality = "USA",
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now,
                RoomId = 1
            };
            var room = new Room { RoomId = 1 };
            var guest = new Guest { Id = 1 };
            _mockGuestRepository.Setup(repo => repo.GetGuestById(1)).Returns(guest);
            _mockRoomRepository.Setup(repo => repo.GetRoomById(1)).Returns(room);
            _mockGuestRepository.Setup(repo => repo.UpdateGuest(It.IsAny<Guest>())).Returns(true);

            // Act
            var result = _controller.UpdateGuest(1, guestDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }
    }
}