using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IMessageHub
{
    public interface IMessageHub
    {
        Task OnNotificationReceived(string message);
        Task OnNewAppointmentReceived(string message);
        Task OnAppointmentStatusUpdated(int appointmentId, AppointmentStatus status, string message);
    }

}
