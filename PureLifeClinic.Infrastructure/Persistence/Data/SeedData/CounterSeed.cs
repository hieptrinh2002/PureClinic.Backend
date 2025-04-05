using PureLifeClinic.Core.Entities.General.Queues;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Infrastructure.Persistence.Data.SeedData
{
    public static class CounterSeed
    {
        public static async Task SeedCounterData(ApplicationDbContext context)
        {
            var counters = new List<Counter>();

            foreach (CounterType type in Enum.GetValues(typeof(CounterType)))
            {
                for (int i = 1; i <= 2; i++)
                {
                    counters.Add(new Counter
                    {
                        Name = $"{type} Counter {i}",
                        CurrentQueueNumber = null,
                        IsActive = true,
                        CounterType = type
                    });
                }
            }

            context.Counters.AddRange(counters);
            await context.SaveChangesAsync();
        }
    }
}
