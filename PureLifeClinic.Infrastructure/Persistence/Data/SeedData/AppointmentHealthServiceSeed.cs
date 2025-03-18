using Bogus;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Persistence.Data.SeedData
{
    public static class AppointmentHealthServiceSeed
    {
        public static IEnumerable<AppointmentHealthService> SeedAppointmentHealthServicesData(
            IEnumerable<Appointment> appointments,
            IEnumerable<HealthService> healthServices)
        {
            var appointmentList = appointments.ToList();
            var healthServiceList = healthServices.ToList();
            var faker = new Faker<AppointmentHealthService>()
                .RuleFor(ahs => ahs.AppointmentId, f => f.PickRandom(appointmentList).Id)
                .RuleFor(ahs => ahs.HealthServiceId, f => f.PickRandom(healthServiceList).Id)

                .RuleFor(ahs => ahs.EntryDate, f => DateTime.Now)
                // Get price at the present time    
                .RuleFor(ahs => ahs.Price, (f, ahs) =>
                {
                    var service = healthServiceList.FirstOrDefault(h => h.Id == ahs.HealthServiceId);
                    return service != null ? service.Price : Math.Round(Convert.ToDouble(f.Random.Double(10.0, 500.0)), 2);
                })
                .RuleFor(ahs => ahs.Quantity, f => f.Random.Int(1, 3));

            return faker.Generate(30);
        }
    }
}
