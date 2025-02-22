using Bogus;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Data.SeedData
{
    public static class MedicalReportSeed
    {
        public static List<MedicalReport> SeedMedicalReportData(Faker faker, List<Medicine> medications)
        {
            return new Faker<MedicalReport>()
                .RuleFor(mr => mr.ReportDate, f => f.Date.Past())
                .RuleFor(mr => mr.Findings, f => f.Lorem.Sentence(10))
                .RuleFor(mr => mr.Recommendations, f => f.Lorem.Sentence(5))
                .RuleFor(mr => mr.Diagnosis, f => f.Lorem.Sentence(7))
                .RuleFor(mr => mr.DoctorNotes, f => f.Lorem.Sentence(8))
                .RuleFor(mr => mr.PrescriptionDetails, f => PrescriptionDetailSeed.SeedPrescriptionDetailData(f, medications))
                .RuleFor(u => u.EntryDate, f => DateTime.Now)
                .Generate(faker.Random.Int(1, 3));
        }
    }
}