using Bogus;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Infrastructure.Persistence.Data.SeedData
{
    public static class UserSeed
    {
        public static IEnumerable<User> SeedUserData(ApplicationDbContext appContext)
        {
            var existingUsernames = new HashSet<string>();
            var existingEmails = new HashSet<string>();

            // Lấy danh sách role từ database
            var roleMapping = appContext.Roles
                .Where(r => new[] { "ADMIN", "DOCTOR", "PATIENT", "EMPLOYEE" }.Contains(r.Code))
                .ToDictionary(r => r.Code, r => r.Id);

            var faker = new Faker();

            var users = new List<User>
            {
                new User
                {
                    FullName = faker.Name.FullName(),
                    UserName = "Admin123",
                    Email = GenerateUniqueEmail(faker, existingEmails),
                    Address = faker.Address.FullAddress(),
                    IsActive = true,
                    RoleId = roleMapping["ADMIN"],
                    DateOfBirth = faker.Date.Past(40, DateTime.Now.AddYears(-18)),
                    Gender = faker.PickRandom<Gender>(),
                    EntryDate = DateTime.Now,
                    EmailConfirmed = true
                },
                new User
                {
                    FullName = faker.Name.FullName(),
                    UserName = "Doctor123",
                    Email = GenerateUniqueEmail(faker, existingEmails),
                    Address = faker.Address.FullAddress(),
                    IsActive = true,
                    RoleId = roleMapping["DOCTOR"],
                    DateOfBirth = faker.Date.Past(40, DateTime.Now.AddYears(-18)),
                    Gender = faker.PickRandom<Gender>(),
                    EntryDate = DateTime.Now,
                    EmailConfirmed = true
                },
                new User
                {
                    FullName = faker.Name.FullName(),
                    UserName = "Patient123",//GenerateUniqueUsername(faker, existingUsernames),
                    Email = GenerateUniqueEmail(faker, existingEmails),
                    Address = faker.Address.FullAddress(),
                    IsActive = true,
                    RoleId = roleMapping["PATIENT"],
                    DateOfBirth = faker.Date.Past(40, DateTime.Now.AddYears(-18)),
                    Gender = faker.PickRandom<Gender>(),
                    EntryDate = DateTime.Now,
                    EmailConfirmed = true
                },
                new User
                {
                    FullName = faker.Name.FullName(),
                    UserName = "Employee123",//GenerateUniqueUsername(faker, existingUsernames),
                    Email = GenerateUniqueEmail(faker, existingEmails),
                    Address = faker.Address.FullAddress(),
                    IsActive = true,
                    RoleId = roleMapping["EMPLOYEE"],
                    DateOfBirth = faker.Date.Past(40, DateTime.Now.AddYears(-18)),
                    Gender = faker.PickRandom<Gender>(),
                    EntryDate = DateTime.Now,
                    EmailConfirmed = true
                }
            };

            return users;
        }

        private static string GenerateUniqueUsername(Faker faker, HashSet<string> existingUsernames)
        {
            string username;
            do
            {
                username = faker.Internet.UserName();
            } while (existingUsernames.Contains(username));
            existingUsernames.Add(username);
            return username;
        }

        private static string GenerateUniqueEmail(Faker faker, HashSet<string> existingEmails)
        {
            string email;
            do
            {
                email = faker.Internet.Email();
            } while (existingEmails.Contains(email));
            existingEmails.Add(email);
            return email;
        }

        //public static IEnumerable<User> SeedUserData(ApplicationDbContext appContext)
        //{
        //    var existingUsernames = new HashSet<string>();
        //    var existingEmails = new HashSet<string>();

        //    var roleIds = appContext.Roles.Select(r => r.Id).ToList();
        //    var faker = new Faker<User>()
        //        .CustomInstantiator(f => new User()) // Khởi tạo User
        //        .RuleFor(u => u.FullName, f => f.Name.FullName())
        //        .RuleFor(u => u.UserName, f =>
        //        {
        //            string username;
        //            do
        //            {
        //                username = f.Internet.UserName();
        //            } while (existingUsernames.Contains(username));
        //            existingUsernames.Add(username);
        //            return username;
        //        })
        //        .RuleFor(u => u.Email, f =>
        //        {
        //            string email;
        //            do
        //            {
        //                email = f.Internet.Email();
        //            } while (existingEmails.Contains(email));
        //            existingEmails.Add(email);
        //            return email;
        //        })
        //        .RuleFor(u => u.Address, f => f.Address.FullAddress())
        //        .RuleFor(u => u.IsActive, f => f.Random.Bool())
        //        .RuleFor(u => u.RoleId, f => roleIds[f.Random.Int(0, roleIds.Count - 1)])
        //        .RuleFor(u => u.DateOfBirth, f => f.Date.Past(40, DateTime.Now.AddYears(-18)))
        //        .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
        //        .RuleFor(u => u.EntryDate, f => DateTime.Now);

        //    return faker.Generate(50);
        //}
    }
}
