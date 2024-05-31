using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace triakl.Models
{
    public class Room
    {
        public int RoomId { get; set; }
        public int Number { get; set; }
        public int Floor { get; set; }
        public string Type { get; set; }

        [JsonIgnore]
        public ICollection<Guest> Guests { get;}
    }
}
