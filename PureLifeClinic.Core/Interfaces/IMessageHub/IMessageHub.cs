using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Core.Interfaces.IMessageHub
{
    public interface IMessageHub
    {
        Task OnNotificationReceived(string message);
        Task OnNewAppointmentReceived(string message);
        Task OnAppointmentStatusUpdated(int appointmentId, AppointmentStatus status, string message);
    }

}
