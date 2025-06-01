using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.PatientsViewModels
{
    public class PatientCreateViewModel
    {
        public string? Notes { get; set; } = string.Empty;
        public bool RequireDeposit { get; set; } = false;   
        public int UserId { get; set; }
        public int? PrimaryDoctorId { get; set; }
        public PatientStatus Status { get; set; } = PatientStatus.New;
    }
}
