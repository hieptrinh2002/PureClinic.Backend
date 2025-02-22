using Bogus;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Data.SeedData
{
    public static class WorkWeekSeed
    {
        public static async Task SeedWorkWeekData(ApplicationDbContext appContext)
        {
            // just suport celander for doctor
            var doctors = appContext.Doctors.ToList();
            var faker = new Faker<WorkWeek>()
                .RuleFor(w => w.UserId, f => doctors[f.Random.Int(0, doctors.Count - 1)].UserId)
                .RuleFor(w => w.WeekStartDate, f =>
                {
                    var currentDate = DateTime.Today;

                    // calculate days until next Monday 
                    int daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)currentDate.DayOfWeek + 7) % 7;

                    // monday of next week  
                    var nextMonday = currentDate.AddDays(daysUntilNextMonday);

                    return nextMonday;
                })

                // end date will be Sunday (7 days after)   
                .RuleFor(w => w.WeekEndDate, (f, w) => w.WeekStartDate.AddDays(6))
                .RuleFor(w => w.EntryDate, f => DateTime.Now);

            for (int i = 0; i < 10; i++)  // create 10 WorkWeek
            {
                var workWeek = faker.Generate(1).First();

                appContext.WorkWeeks.Add(workWeek);
                await appContext.SaveChangesAsync();

                for (int day = 0; day < 7; day++)
                {
                    var workDay1 = new WorkDay
                    {
                        WorkWeekId = workWeek.Id,
                        DayOfWeek = (DayOfWeek)((int)workWeek.WeekStartDate.DayOfWeek + day % 7),
                        StartTime = new TimeSpan(new Random().Next(8, 9), 0, 0),  // 8-9 AM
                        EndTime = new TimeSpan(new Random().Next(11, 12), 0, 0),  //  11-12 PM
                        Notes = new Faker().Lorem.Sentence(),
                        EntryDate = DateTime.Now
                    };
                    var workDay2 = new WorkDay
                    {
                        WorkWeekId = workWeek.Id,
                        DayOfWeek = (DayOfWeek)((int)workWeek.WeekStartDate.DayOfWeek + day % 7),
                        StartTime = new TimeSpan(new Random().Next(13, 15), 0, 0),  // 13-15 PM
                        EndTime = new TimeSpan(new Random().Next(17, 21), 0, 0),  //  5-9 PM
                        Notes = new Faker().Lorem.Sentence(),
                        EntryDate = DateTime.Now
                    };

                    appContext.WorkDays.Add(workDay1);
                    appContext.WorkDays.Add(workDay2);
                }
            }
            await appContext.SaveChangesAsync();
        }
    }
}
