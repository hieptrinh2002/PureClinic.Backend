using Bogus;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Persistence.Data.SeedData
{
    public static class PatientSeed
    {
        public static Patient SeedPatientsData(int userId)
        {
            var patient = new Faker<Patient>().RuleFor(p => p.Notes, f => f.Lorem.Paragraph())
                                       .RuleFor(p => p.UserId, f => userId)
                                       .RuleFor(p => p.EntryDate, f => DateTime.Now)
                                       .Generate(1)
                                       .FirstOrDefault();
            return patient;
        }
    }
}
