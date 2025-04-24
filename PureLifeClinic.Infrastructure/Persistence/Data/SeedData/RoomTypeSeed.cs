
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Persistence.Data.SeedData
{
    public static class RoomTypeSeed
    {
        public static List<RoomType> SeedRoomTypeData()
        {
            return new List<RoomType>{
                new RoomType
                {
                    RoomTypeName = "Khám tổng quát",
                    Description = "Phòng khám tổng quát",
                    IsActive = true,
                    Rooms = new List<Room>
                    {
                        new Room { RoomName = "Phòng 101", RoomNumber = 101, Description = "Khám tổng quát 1", IsActive = true },
                        new Room { RoomName = "Phòng 102", RoomNumber = 102, Description = "Khám tổng quát 2", IsActive = true }
                    }
                },
                new RoomType
                {
                    RoomTypeName = "Khám nội",
                    Description = "Phòng khám nội khoa",
                    IsActive = true,
                    Rooms = new List<Room>
                    {
                        new Room { RoomName = "Phòng 201", RoomNumber = 201, Description = "Khám nội 1", IsActive = true },
                        new Room { RoomName = "Phòng 202", RoomNumber = 202, Description = "Khám nội 2", IsActive = true }
                    }
                },
                new RoomType
                {
                    RoomTypeName = "Khám ngoại",
                    Description = "Phòng khám ngoại khoa",
                    IsActive = true,
                    Rooms = new List<Room>
                    {
                        new Room { RoomName = "Phòng 301", RoomNumber = 301, Description = "Khám ngoại 1", IsActive = true },
                        new Room { RoomName = "Phòng 302", RoomNumber = 302, Description = "Khám ngoại 2", IsActive = true }
                    }
                },
                new RoomType
                {
                    RoomTypeName = "Sản phụ khoa",
                    Description = "Phòng khám sản phụ",
                    IsActive = true,
                    Rooms = new List<Room>
                    {
                        new Room { RoomName = "Phòng 401", RoomNumber = 401, Description = "Sản phụ khoa 1", IsActive = true },
                        new Room { RoomName = "Phòng 402", RoomNumber = 402, Description = "Sản phụ khoa 2", IsActive = true }
                    }
                },
                new RoomType
                {
                    RoomTypeName = "Nhi khoa",
                    Description = "Phòng khám nhi",
                    IsActive = true,
                    Rooms = new List<Room>
                    {
                        new Room { RoomName = "Phòng 501", RoomNumber = 501, Description = "Nhi khoa 1", IsActive = true },
                        new Room { RoomName = "Phòng 502", RoomNumber = 502, Description = "Nhi khoa 2", IsActive = true }
                    }
                }
            };
        }
    }
}
