using Bogus;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Data.SeedData
{
    public static class DoctorSeed
    {
        public static Doctor SeedOneDoctorsData(int userId)
        {
            var doctor = new Faker<Doctor>()
                .RuleFor(d => d.Specialty, f => f.Name.JobTitle())
                .RuleFor(d => d.Qualification, f => f.Name.JobDescriptor())
                .RuleFor(d => d.ExperienceYears, f => f.Random.Int(1, 30))
                .RuleFor(d => d.Description, f => f.Lorem.Paragraph())
                .RuleFor(d => d.RegistrationNumber, f => f.Random.Guid().ToString())
                .RuleFor(d => d.UserId, f => userId)
                .RuleFor(d => d.EntryDate, f => DateTime.Now).Generate(1).FirstOrDefault();
            return doctor;
        }
    }
}
