using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Core.Interfaces.IMessageHub
{
    public interface IAppointmentClient
    {
        Task AppointmentStatusUpdated(int appointmentId, AppointmentStatus status, string message);
    }
}
