using Bogus;
using Microsoft.Extensions.DependencyInjection;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Infrastructure.Persistence.Data;

namespace PureLifeClinic.Infrastructure.Persistence.Data.SeedData
{
    public static class AppointmentSeed
    {
        public static IEnumerable<Appointment> SeedAppointmentData(IServiceProvider serviceProvider)
        {
            var appContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var doctors = appContext.Doctors.ToList();
            var patients = appContext.Patients.ToList();
            var medicines = appContext.Medicines.ToList();

            var faker = new Faker<Appointment>()
                .RuleFor(a => a.AppointmentDate, f => f.Date.Future())
                .RuleFor(a => a.Reason, f => f.Lorem.Sentence(8))
                .RuleFor(a => a.PatientId, f => patients[f.Random.Int(0, patients.Count - 1)].Id)
                .RuleFor(a => a.DoctorId, f => doctors[f.Random.Int(0, doctors.Count - 1)].Id)
                .RuleFor(a => a.Status, f => f.PickRandom<AppointmentStatus>())
                .RuleFor(a => a.EntryDate, f => DateTime.Now)
                .RuleFor(a => a.MedicalReports, f => MedicalReportSeed.SeedMedicalReportData(f, medicines));
            return faker.Generate(15);
        }
    }
}
