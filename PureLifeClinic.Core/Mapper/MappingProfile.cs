using AutoMapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // RefreshToken mappings
            CreateMap<RefreshToken, RefreshTokenViewModel>().ReverseMap();
            CreateMap<RefreshTokenCreateViewModel, RefreshToken>().ReverseMap();

            // Product mappings
            CreateMap<Product, ProductViewModel>().ReverseMap();
            CreateMap<ProductCreateViewModel, Product>().ReverseMap();
            CreateMap<ProductUpdateViewModel, Product>().ReverseMap();

            // Role mappings
            CreateMap<Role, RoleViewModel>().ReverseMap();
            CreateMap<RoleCreateViewModel, Role>().ReverseMap();
            CreateMap<RoleUpdateViewModel, Role>().ReverseMap();

            // User mappings
            CreateMap<User, UserViewModel>().ReverseMap();
            CreateMap<User, PatientViewModel>().ReverseMap();

            //Schedule
            CreateMap<WorkWeek, WorkWeekScheduleViewModel>().ReverseMap();

            CreateMap<User, DoctorViewModel>()
             .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null))
             .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Specialty : null))
             .ForMember(dest => dest.Qualification, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Qualification : null))
             .ForMember(dest => dest.ExperienceYears, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.ExperienceYears : null))
             .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.Description : null))
             .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(src => src.Doctor != null ? src.Doctor.RegistrationNumber : null));

            //Appointment mappings
            CreateMap<Appointment, AppointmentCreateViewModel>().ReverseMap();
            CreateMap<Appointment, AppointmentUpdateViewModel>().ReverseMap();
            CreateMap<Appointment, DoctorAppointmentViewModel>()
                .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient.User)).ReverseMap();
            CreateMap<Appointment, PatientAppointmentViewModel>()
                .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => src.Doctor.User)).ReverseMap();
            CreateMap<Appointment, AppointmentViewModel>()
              .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
              .ForMember(dest => dest.Patient, opt => opt.MapFrom(src => src.Patient.User))
              .ForMember(dest => dest.Doctor, opt => opt.MapFrom(src => src.Doctor.User))
              .ReverseMap();

            //Medical report mappings
            CreateMap<MedicalReport, MedicalReportViewModel>()
                .ForMember(dest => dest.MedicalFiles, opt => opt.MapFrom(src => src.MedicalFiles != null ? src.MedicalFiles.Select(m => m.FilePath).ToList() : new List<string>()))
                .ForMember(dest => dest.PrescriptionDetails, opt => opt.MapFrom(src => src.PrescriptionDetails))
                .ReverseMap();

            CreateMap<MedicalReport, MedicalReportCreateViewModel>().ReverseMap();
            CreateMap<MedicalReport, MedicalReportUpdateViewModel>().ReverseMap()
              .ForMember(dest => dest.PrescriptionDetails, opt => opt.MapFrom(src => src.PrescriptionDetails));


            // Prescription details view model
            CreateMap<PrescriptionDetail, PrescriptionDetailViewModel>().ReverseMap()
                .ForMember(dest => dest.Medicine, opt => opt.MapFrom(src => src.Medicine));

            CreateMap<PrescriptionDetail, PrescriptionDetailCreateViewModel>().ReverseMap();
            CreateMap<PrescriptionDetail, PrescriptionDetailUpdateViewModel>().ReverseMap()
                .ForMember(dest => dest.Medicine, opt => opt.MapFrom(src => src.Medicine));

            //Medicine mappings
            CreateMap<Medicine, MedicineViewModel>().ReverseMap();
            CreateMap<Medicine, MedicineCreateViewModel>().ReverseMap();
            CreateMap<Medicine, MedicineUpdateViewModel>().ReverseMap();

            //Medicine mappings
            CreateMap<Medicine, MedicineViewModel>().ReverseMap();
            CreateMap<MedicineCreateViewModel, Medicine>().ReverseMap();
            CreateMap<MedicineUpdateViewModel, Medicine>().ReverseMap();

            //Medical file mappins
            CreateMap<MedicalFile, MedicalFileUpdateViewModel>().ReverseMap();
            CreateMap<MedicalFile, MedicalFileCreateViewModel>().ReverseMap();

            //Invoice mappings
            CreateMap<Invoice, InvoiceViewModel>().ReverseMap();
            CreateMap<Invoice, InvoiceCreateViewModel>().ReverseMap();
        }
    }
}
