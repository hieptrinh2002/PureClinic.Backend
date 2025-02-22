﻿using Bogus;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Data.SeedData
{
    public static class UserSeed
    {
        public static IEnumerable<User> SeedUserData(ApplicationDbContext appContext)
        {
            var existingUsernames = new HashSet<string>();
            var existingEmails = new HashSet<string>();

            var roleIds = appContext.Roles.Select(r => r.Id).ToList();
            var faker = new Faker<User>()
                .CustomInstantiator(f => new User()) // Khởi tạo User
                .RuleFor(u => u.FullName, f => f.Name.FullName())
                .RuleFor(u => u.UserName, f =>
                {
                    string username;
                    do
                    {
                        username = f.Internet.UserName();
                    } while (existingUsernames.Contains(username));
                    existingUsernames.Add(username);
                    return username;
                })
                .RuleFor(u => u.Email, f =>
                {
                    string email;
                    do
                    {
                        email = f.Internet.Email();
                    } while (existingEmails.Contains(email));
                    existingEmails.Add(email);
                    return email;
                })
                .RuleFor(u => u.Address, f => f.Address.FullAddress())
                .RuleFor(u => u.IsActive, f => f.Random.Bool())
                .RuleFor(u => u.RoleId, f => roleIds[f.Random.Int(0, roleIds.Count - 1)])
                .RuleFor(u => u.DateOfBirth, f => f.Date.Past(40, DateTime.Now.AddYears(-18)))
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                .RuleFor(u => u.EntryDate, f => DateTime.Now);

            return faker.Generate(50);
        }
    }
}
