using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider, ILoggerFactory loggerFactory, int? retry = 0)
        {
            int retryForAvailability = retry ?? 0;
            var appContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var UserManager = serviceProvider.GetRequiredService<UserManager<User>>();
            try
            {
                // Adding Roles
                if (!appContext.Roles.Any())
                {
                    using (var transaction = appContext.Database.BeginTransaction())
                    {
                        appContext.Roles.AddRange(Roles());
                        await appContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                }

                // Adding Users
                if (!appContext.Users.Any())
                {
                    var defaultUser = new User { FullName = "Kawser Hamid", UserName = "hamid", RoleId = 1, Email = "kawser2133@gmail.com", EntryDate = DateTime.Now, IsActive = true };
                    IdentityResult userResult = await UserManager.CreateAsync(defaultUser, "Hamid@12");
                    if (userResult.Succeeded)
                    {
                        // here we assign the new user role 
                        await UserManager.AddToRoleAsync(defaultUser, "ADMIN");
                    }
                }

                // Adding Products
                if (!appContext.Products.Any())
                {
                    using (var transaction = appContext.Database.BeginTransaction())
                    {
                        appContext.Products.AddRange(Products());
                        await appContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                }

                // Adding Products
                if (!appContext.Doctors.Any())
                {
                    using (var transaction = appContext.Database.BeginTransaction())
                    {
                        appContext.Doctors.AddRange(GenerateDoctors());
                        await appContext.SaveChangesAsync();
                        transaction.Commit();
                    }
                }

            }
            catch (Exception ex)
            {
                if (retryForAvailability < 10)
                {
                    retryForAvailability++;
                    var log = loggerFactory.CreateLogger<ApplicationDbContextSeed>();
                    log.LogError(ex.Message);
                    await SeedAsync(serviceProvider, loggerFactory, retryForAvailability);
                }
            }
        }

        /************* Prerequisite for Start Application ********/

        static IEnumerable<Role> Roles()
        {
            return new List<Role>
            {
                new Role {Code="PATIENT", Name = "Patient", NormalizedName= "PATIENT", IsActive = true, EntryDate= DateTime.Now },
                new Role {Code="ADMIN", Name = "Admin", NormalizedName="ADMIN", IsActive = true, EntryDate= DateTime.Now },
                new Role {Code="DOCTOR", Name = "Doctor", NormalizedName= "DOCTOR", IsActive = true, EntryDate= DateTime.Now },
            };
        }

        static IEnumerable<Product> Products()
        {
            var faker = new Faker<Product>()
                .RuleFor(c => c.Code, f => f.Commerce.Product())
                .RuleFor(c => c.Name, f => f.Commerce.ProductName())
                .RuleFor(c => c.Description, f => f.Commerce.ProductDescription())
                .RuleFor(c => c.Price, f => Convert.ToDouble(f.Commerce.Price(1, 1000, 0)))
                .RuleFor(c => c.Quantity, f => f.Commerce.Random.Number(100))
                .RuleFor(c => c.IsActive, f => f.Random.Bool())
                .RuleFor(c => c.EntryDate, DateTime.Now);

            return faker.Generate(100);

        }

        static IEnumerable<Doctor> GenerateDoctors()
        {
            var faker = new Faker<Doctor>()
                .RuleFor(d => d.Specialty, f => f.PickRandom(new[]
                {
                    "Internal Medicine",
                    "Pediatrics",
                    "Cardiology",
                    "Dermatology",
                    "Neurology"
                }))
                .RuleFor(d => d.Qualification, f => $"{f.Name.Prefix()} in {f.Company.CatchPhrase()}")
                .RuleFor(d => d.ExperienceYears, f => f.Random.Int(1, 40))
                .RuleFor(d => d.Description, f => f.Lorem.Paragraphs(1, 3))
                .RuleFor(d => d.RegistrationNumber, f => f.Random.String2(8, 12, "ABCDEFGHIJKLMNOPQR6789"))
                .RuleFor(d => d.UserId, f => 34); // Assuming UserId references existing user IDs

            return faker.Generate(1);
        }

    }
}
