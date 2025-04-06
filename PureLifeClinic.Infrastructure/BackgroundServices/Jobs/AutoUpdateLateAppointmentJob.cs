using Hangfire;
using PureLifeClinic.Application.Interfaces.IBackgroundJobs;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Infrastructure.BackgroundServices.Jobs
{
    public class AutoUpdateLateAppointmentJob : IAutoUpdateLateAppointmentJob
    {
        private readonly IMailService _emailService;
        private readonly IUnitOfWork _unitOfWork;

        public AutoUpdateLateAppointmentJob(
            IMailService emailService,
            IUnitOfWork unitOfWork)
        {
            _emailService = emailService;
            _unitOfWork = unitOfWork;
        }

        [AutomaticRetry(Attempts = 3, OnAttemptsExceeded = AttemptsExceededAction.Fail)]
        public async Task ExecAutoUpdateLateAppointmentJob()
        {
            List<Appointment> lateAppointments = await _unitOfWork.Appointments.GetLateAppointments(DateTime.Now);

            foreach (var appt in lateAppointments)
            {
                // 1. update status
                appt.UpdatedDate = DateTime.Now;
                appt.Status = Core.Enums.AppointmentStatus.NoShowCanceled;

                // 2. send reminder email
                if (appt.Patient.User?.Email != null)
                    await _emailService.SendNoShowReminder(appt.Patient.User.Email, appt.AppointmentDate);

                // 3. count not show time
                int missedCount = await _unitOfWork.Appointments.CountConsecutiveMissedAppointments(appt.PatientId);
                if (missedCount >= 3)
                {
                    var patient = await _unitOfWork.Patients.GetById(appt.PatientId, default);
                    if (patient != null)
                    {
                        patient.RequireDeposit = true;  
                    }
                }
            }
        }
    }
}
