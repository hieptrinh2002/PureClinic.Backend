using Bogus;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Data.SeedData
{
    public static class HealthServiceSeed
    {
        public static IEnumerable<HealthService> SeedHealthServicesData()
        {
            var serviceNames = new List<string>
            {
                "Khám Tổng Quát",
                "Khám Chuyên Khoa",
                "Xét Nghiệm Máu",
                "Siêu Âm",
                "Chụp X-Quang",
                "Khám Tai Mũi Họng",
                "Khám Tim Mạch",
                "Tư Vấn Sức Khỏe",
                "Điều Trị Nội Khoa",
                "Điều Trị Ngoại Khoa"
            };

            var faker = new Faker<HealthService>()
                .RuleFor(h => h.Name, f => f.PickRandom(serviceNames))
                .RuleFor(h => h.Description, f => f.Lorem.Sentence(10))
                // HierarchyPath: Node gốc được đánh dấu bằng "/"
                .RuleFor(h => h.HierarchyPath, f => "/")
                .RuleFor(h => h.Price, f => Math.Round(Convert.ToDouble(f.Random.Double(10.0, 500.0)), 2))
                .RuleFor(h => h.IsActive, f => f.Random.Bool())
                .RuleFor(h => h.EntryDate, f => DateTime.Now);

            return faker.Generate(10);
        }
    }
}
