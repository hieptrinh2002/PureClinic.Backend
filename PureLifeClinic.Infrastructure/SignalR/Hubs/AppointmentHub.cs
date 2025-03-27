using Microsoft.AspNetCore.SignalR;
using PureLifeClinic.Application.Interfaces.IMessageHub;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Infrastructure.SignalR.Hubs
{

    public class AppointmentHub : Hub<IAppointmentClient>
    {
        public async Task UpdateAppointmentStatus(int appointmentId, AppointmentStatus status, string message)
        {
            await Clients.All.AppointmentStatusUpdated(appointmentId, status, message);
        }
    }

    // Khi bệnh nhân đặt lịch hẹn, hệ thống sẽ thông báo ngay cho nhân viên tiếp tân.
    // Khi bác sĩ xác nhận lịch hẹn, bệnh nhân sẽ nhận được thông báo ngay lập tức.
    // Khi bệnh nhân hủy lịch, thông tin sẽ cập nhật ngay trên màn hình quản
}
