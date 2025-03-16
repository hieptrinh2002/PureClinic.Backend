using Hangfire;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IBackgroundJobs;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.BackgroundServices.Jobs
{
    public class AppointmentReminderJob : IAppointmentReminderJob
    {
        private readonly IMailService _emailService;
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentReminderJob(IMailService emailService, IAppointmentRepository appointmentRepository)
        {
            _emailService = emailService;
            _appointmentRepository = appointmentRepository;
        }

        public async Task SendRemindersAsync()
        {
            List<Appointment> appointments = new();//await _appointmentRepository.GetUpcomingAppointmentsAsync();
            foreach (var appointment in appointments)
            {
                MailRequestViewModel model = new MailRequestViewModel
                {
                    ToEmail = appointment.Patient.User?.Email,
                    Subject = "Nhắc lịch khám",
                    Body = "Bạn có lịch khám sắp tới."
                };

                BackgroundJob.Enqueue<IMailService>(emailService => emailService.SendEmailAsync(model));
            }
        }
    }
}
