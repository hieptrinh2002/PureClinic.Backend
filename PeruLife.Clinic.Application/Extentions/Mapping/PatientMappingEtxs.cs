using PureLifeClinic.Application.BusinessObjects.PatientsViewModels;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.Extentions.Mapping
{
    public static class PatientMappingEtxs
    {
        public static PatientViewModel ToPatientViewModel(this Patient patient)
        {
            if(patient.User == null)
                throw new ArgumentNullException(nameof(patient.User), "User information is required to map to PatientViewModel.");

            return new PatientViewModel
            {
                PatientId = patient.Id,
                FullName = patient.User.FullName,    
                UserName = patient.User.UserName,
                Email = patient.User.Email,
                Address = patient.User.Address,
                PhoneNumber = patient.User.PhoneNumber,
                Gender = patient.User.Gender,   
                DateOfBirth = patient.User.DateOfBirth
            };
        }

        public static Patient MapToPatient(this PatientCreateViewModel model)
        {
            if(model == null)
                throw new ArgumentNullException(nameof(model), "PatientCreateViewModel cannot be null.");

            return new Patient
            {
                Notes = model.Notes,
                RequireDeposit = model.RequireDeposit,
                UserId = model.UserId, 
                PatientStatus = model.Status,
                PrimaryDoctorId = model.PrimaryDoctorId
            };
        }
    }
}
