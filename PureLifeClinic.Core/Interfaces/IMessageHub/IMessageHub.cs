using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IMessageHub
{
    public interface IMessageHub
    {
        Task ReceiveNotification(string message);
        Task ReceiveNewAppointment(string message);
        Task AppointmentStatusUpdated(int appointmentId, AppointmentStatus status, string message);
    }
}
