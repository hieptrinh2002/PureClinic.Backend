using Microsoft.AspNetCore.Http;

namespace PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Request
{
    public class DoctorUpdateProfileRequestVM
    {
        public IFormFile? Avatar { get; set; }  
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Specialty { get; set; }

        public string Qualification { get; set; }

        public int ExperienceYears { get; set; }

        public string Description { get; set; }

        public string RegistrationNumber { get; set; }
    }
}
