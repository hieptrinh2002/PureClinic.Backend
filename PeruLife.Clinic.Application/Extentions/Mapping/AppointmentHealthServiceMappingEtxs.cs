using PureLifeClinic.Application.BusinessObjects.AppointmentHealthServices;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.Extentions.Mapping
{
    public static class AppointmentHealthServiceMappingEtxs
    {
        public static AppointmentHealthServiceVM MapToAppointmentHealthServiceViewModel(this AppointmentHealthService entity)
        {
            return new AppointmentHealthServiceVM
            {
                AppointmentId = entity.AppointmentId,
                HealthServiceName = entity.HealthService?.Name ?? string.Empty,
                HealthServiceId = entity.HealthServiceId,
                RoomId = entity.RoomId,
                RoomName = entity.Room?.RoomName ?? string.Empty,
                Status = entity.Status,
                Price = entity.Price
            };
        }

        public static List<AppointmentHealthServiceVM> MapToAppointmentHealthServiceViewModelList(this List<AppointmentHealthService> entities)
        {
            return entities.Select(entity => new AppointmentHealthServiceVM
            {
                AppointmentId = entity.AppointmentId,
                HealthServiceName = entity.HealthService?.Name ?? string.Empty,
                HealthServiceId = entity.HealthServiceId,
                RoomId = entity.RoomId,
                RoomName = entity.Room?.RoomName ?? string.Empty,
                Status = entity.Status,
                Price = entity.Price
            }).ToList();
        }

        public static AppointmentHealthService MapToAppointmentHealthService(this AppointmentHealthServiceCreateVM model)
        {
            return new AppointmentHealthService
            {
                AppointmentId = model.AppointmentId,
                HealthServiceId = model.HealthServiceId,
                RoomId = model.RoomId,
                Status = model.Status,
                Price = model.Price,
                SortOrder = model.SortOrder
            };
        }
    }
}
