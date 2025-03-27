using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.Interfaces.IMessageHub
{
    public interface IAppointmentClient
    {
        Task AppointmentStatusUpdated(int appointmentId, AppointmentStatus status, string message);
    }
}
