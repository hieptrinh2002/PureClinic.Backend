using Bogus;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Data.SeedData
{
    public static class MedicineSeed
    {
        public static IEnumerable<Medicine> SeedMedicineData()
        {
            var faker = new Faker<Medicine>()
                .RuleFor(m => m.Name, f => f.Commerce.ProductName())
                .RuleFor(m => m.Description, f => f.Lorem.Sentence(10))
                .RuleFor(m => m.Price, f => Math.Round(f.Random.Double(5.0, 100.0), 2))
                .RuleFor(m => m.Quantity, f => f.Random.Int(0, 500))
                .RuleFor(m => m.Manufacturer, f => f.Company.CompanyName())
                .RuleFor(m => m.EntryDate, f => DateTime.Now)
                .RuleFor(m => m.Code, f => (new Guid()).ToString().Substring(0, 7));

            return faker.Generate(60);
        }
    }
}
