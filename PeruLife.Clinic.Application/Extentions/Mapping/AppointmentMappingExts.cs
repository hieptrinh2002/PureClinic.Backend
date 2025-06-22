using PureLifeClinic.Application.BusinessObjects.AppointmentViewModels.Response;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.Extentions.Mapping
{
    public static class AppointmentMappingExts
    {
        public static AppointmentViewModel MapToAppointmentViewModel(this Appointment entity)
        {
            return new AppointmentViewModel
            {
                Id = entity.Id,
                AppointmentDate = entity.AppointmentDate,
                Reason = entity.Reason,
                Status = entity.Status,
                Patient = entity.Patient.ToPatientViewModel(),
            };
        }
    }
}
