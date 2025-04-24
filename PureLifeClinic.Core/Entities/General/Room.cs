using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class Room: Base<int>
    {
        public string RoomName { get; set; } = string.Empty;

        public int RoomNumber { get; set; } = 0;    

        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public RoomType RoomType { get; set; } // FK to RoomType   

        [ForeignKey(nameof(RoomTypeId))]
        public int RoomTypeId { get; set; } // FK to RoomType
    }

    public class RoomType: Base<int>
    {
        public string RoomTypeName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;

        public ICollection<Room>? Rooms { get; set; } = new List<Room>();
    }
}
