using Bogus;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Persistence.Data.SeedData
{
    public static class PrescriptionDetailSeed
    {
        public static List<PrescriptionDetail> SeedPrescriptionDetailData(Faker faker, List<Medicine> medications)
        {
            return new Faker<PrescriptionDetail>()
                .RuleFor(pd => pd.Quantity, f => f.Random.Int(1, 5))
                .RuleFor(pd => pd.Dosage, f => $"{f.Random.Int(1, 3)} lần/ngày")
                .RuleFor(pd => pd.Instructions, f => f.Lorem.Sentence(5))
                .RuleFor(pd => pd.MedicineId, f => medications[f.Random.Int(0, medications.Count - 1)].Id)
                .RuleFor(u => u.EntryDate, f => DateTime.Now)
                .Generate(faker.Random.Int(1, 5));
        }
    }
}
