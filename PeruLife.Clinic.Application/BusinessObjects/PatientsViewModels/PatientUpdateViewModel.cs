using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.PatientsViewModels
{
    public class PatientUpdateViewModel
    {
        public string? Notes { get; set; }
        public bool RequireDeposit { get; set; }
        public int? PrimaryDoctorId { get; set; }
    }
}
