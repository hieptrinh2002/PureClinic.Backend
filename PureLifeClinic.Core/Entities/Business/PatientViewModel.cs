using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Entities.Business
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }

        public string Role { get; set; }

    }
}
