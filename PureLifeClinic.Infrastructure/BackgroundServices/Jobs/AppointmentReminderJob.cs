using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IBackgroundJobs;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Infrastructure.BackgroundServices.Jobs
{
    public class AppointmentReminderJob : IAppointmentReminderJob
    {
        private readonly IBackgroundJobService _backgroundJobService;
        private readonly IAppointmentRepository _appointmentRepository;

        public AppointmentReminderJob(IAppointmentRepository appointmentRepository, IBackgroundJobService backgroundJobService)
        {
            _appointmentRepository = appointmentRepository;
            _backgroundJobService = backgroundJobService;
        }

        public async Task SendRemindersAsync(int hoursBefore)
        {
            int batchSize = 100;
            int pageIndex = 0;

            while (true)
            {
                List<Appointment> appointments = await _appointmentRepository.GetUpcomingAppointmentsBatchAsync(pageIndex, batchSize, hoursBefore);

                if (appointments.Count == 0)
                    break;

                var emailList = new List<MailRequestViewModel>();

                foreach (var appointment in appointments)
                {
                    if (appointment.Patient?.User?.Email != null)
                    {
                        emailList.Add(new MailRequestViewModel
                        {
                            ToEmail = appointment.Patient.User.Email,
                            Subject = "Appoinment booking reminder",
                            Body = $"You have a appointment with doctor {appointment.Doctor.User.FullName} at {appointment.AppointmentDate:dd/MM/yyyy HH:mm}, " +
                                   $"(Time remain - {Math.Round((appointment.AppointmentDate - DateTime.UtcNow).TotalHours)} h)."
                        });
                    }
                }

                if (emailList.Count > 0)
                {
                    _backgroundJobService.ScheduleImmediateJob<IMailService>((emailService) => emailService.SendEmailBatchAsync(emailList));
                }

                pageIndex++;
            }
        }
    }
}
