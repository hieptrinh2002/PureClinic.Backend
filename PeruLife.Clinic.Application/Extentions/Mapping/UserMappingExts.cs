using PureLifeClinic.Application.BusinessObjects.DoctorViewModels.Response;
using PureLifeClinic.Application.BusinessObjects.UserViewModels;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Application.Extentions.Mapping
{
    public static class UserMappingExts
    {
        public static UserViewModel MapToUserViewModel(this User entity)
        {
            return new UserViewModel
            {
                Id = entity.Id,
                FullName = entity.FullName,
                UserName = entity.UserName,
                Email = entity.Email,
                Role = entity.Role.Name,
                Address = entity.Address,
                Gender = entity.Gender,
                DateOfBirth = entity.DateOfBirth
            };
        }

        public static List<UserViewModel> MapToListUserViewModel(this List<User> entites)
        {
            return entites.Select(entity => new UserViewModel
            {
                Id = entity.Id,
                FullName = entity.FullName,
                UserName = entity.UserName,
                Email = entity.Email,
                Role = entity.Role.Name,
                Address = entity.Address,
                Gender = entity.Gender,
                DateOfBirth = entity.DateOfBirth
            }).ToList();
        }

        public static DoctorViewModel MapToDoctorViewModel(this User entity)
        {
            if(entity.Doctor == null)
                throw new ArgumentNullException(nameof(entity.Doctor), "Doctor information is not available.");

            return new DoctorViewModel
            {
                Id = entity.Doctor.Id,
                ImagePath = entity.ImagePath,
                FullName = entity.FullName,
                Email = entity.Email,
                Specialty = entity.Doctor.Specialty,
                Qualification = entity.Doctor.Qualification,
                ExperienceYears = entity.Doctor.ExperienceYears,
                Description = entity.Doctor.Description,
                RegistrationNumber = entity.Doctor.RegistrationNumber,
                Role = entity.Role.Name,
                PhoneNumber = entity.PhoneNumber,
                SuccessfulPatients = entity.Doctor.SuccessfulPatients,
            };
        }

        public static List<DoctorViewModel> MapToListDoctorViewModel(this List<User> entites)
        {
            if (entites.Any(entity => entity.Doctor == null))
                throw new ArgumentNullException(nameof(entites), "Doctor information is not available for one or more users.");

            return entites.Select(entity => new DoctorViewModel
            {
                Id = entity.Doctor!.Id,
                ImagePath = entity.ImagePath,
                FullName = entity.FullName,
                Email = entity.Email,
                Specialty = entity.Doctor.Specialty,
                Qualification = entity.Doctor.Qualification,
                ExperienceYears = entity.Doctor.ExperienceYears,
                Description = entity.Doctor.Description,
                RegistrationNumber = entity.Doctor.RegistrationNumber,
                Role = entity.Role.Name,
                PhoneNumber = entity.PhoneNumber,
                SuccessfulPatients = entity.Doctor.SuccessfulPatients,
            }).ToList();
        }
    }
}
