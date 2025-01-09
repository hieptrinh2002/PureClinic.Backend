using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Infrastructure.Data
{
    public class ApplicationDbContextSeed
    {
        private static string GenerateStrongPassword()
        {
            var faker = new Faker();
            return "ROnaldo11nbk..";
        }

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
                    using var transaction = appContext.Database.BeginTransaction();
                    appContext.Roles.AddRange(Roles());
                    await appContext.SaveChangesAsync();
                    transaction.Commit();
                }

                // Adding Users
                if (!appContext.Users.Any())
                {
                    var r = appContext.Roles.FirstOrDefault(r => r.NormalizedName == "ADMIN");
                    var defaultUser = new User
                    {
                        FullName =
                        "Kawser Hamid",
                        UserName = "hamid",
                        RoleId = r.Id,
                        Email = "kawser2133@gmail.com",
                        EntryDate = DateTime.Now,
                        IsActive = true
                    };

                    IdentityResult userResult = await UserManager.CreateAsync(defaultUser, "Hamid@12");
                    if (userResult.Succeeded)
                    {
                        await UserManager.AddToRoleAsync(defaultUser, "ADMIN");
                    }
                    var roles = appContext.Roles.ToList();
                    var users = Users(appContext).ToList();
                    foreach (var user in users)
                    {
                        
                        var role = roles.FirstOrDefault(r => r.Id == user.RoleId);
                        if (role == null) continue; 

                        var defaultPassword = GenerateStrongPassword();
                        IdentityResult result = await UserManager.CreateAsync(user, defaultPassword);

                        if (result.Succeeded)
                        {
                            await UserManager.AddToRoleAsync(user, role.NormalizedName);

                            if (role.NormalizedName == "DOCTOR")
                            {
                                var doctor = new Faker<Doctor>()
                                           .RuleFor(d => d.Specialty, f => f.Name.JobTitle())
                                           .RuleFor(d => d.Qualification, f => f.Name.JobDescriptor())
                                           .RuleFor(d => d.ExperienceYears, f => f.Random.Int(1, 30))
                                           .RuleFor(d => d.Description, f => f.Lorem.Paragraph())
                                           .RuleFor(d => d.RegistrationNumber, f => f.Random.Guid().ToString())
                                           .RuleFor(d => d.UserId, f => user.Id)
                                           .RuleFor(d => d.EntryDate, f => DateTime.Now).Generate(1).FirstOrDefault();

                                appContext.Doctors.Add(doctor);

                            }
                            else if (role.NormalizedName == "PATIENT")
                            {
                                var patient = new Faker<Patient>()
                                           //.RuleFor(p => p.MedicalHistory, f => f.Lorem.Sentence())
                                           .RuleFor(p => p.Notes, f => f.Lorem.Paragraph())
                                           .RuleFor(p => p.UserId, f => user.Id)
                                           .RuleFor(p => p.EntryDate, f => DateTime.Now).Generate(1).FirstOrDefault();

                                appContext.Patients.Add(patient);
                            }
                            else if (role.NormalizedName == "EMPLOYEE")
                            {

                            }
                            else if (role.NormalizedName == "ADMIN")
                            {

                            }
                        }
                        else
                        {
                            var t = $"Failed to create user: {user.UserName}, Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}";
                            Console.WriteLine($"Failed to create user: {user.UserName}, Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                        }
                    }
                }

                // Adding Medication
                if (!appContext.Medications.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    appContext.Medications.AddRange(Medications());
                    await appContext.SaveChangesAsync();
                    transaction.Commit();
                }

                // Adding appoinments
                if (!appContext.Appointments.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    appContext.Appointments.AddRange(Appointments(serviceProvider));
                    await appContext.SaveChangesAsync();
                    transaction.Commit();
                }

                // Adding Schedule
                if (!appContext.WorkWeeks.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    await WorkWeeks(appContext);
                    transaction.Commit();
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
                new Role {Code="ADMIN", Name = "Admin", NormalizedName="ADMIN", IsActive = true, EntryDate= DateTime.Now },
                new Role {Code="EMPLOYEE", Name = "Employee", NormalizedName="EMPLOYEE", IsActive = true, EntryDate= DateTime.Now },
                new Role {Code="PATIENT", Name = "Patient", NormalizedName= "PATIENT", IsActive = true, EntryDate= DateTime.Now },
                new Role {Code="DOCTOR", Name = "Doctor", NormalizedName= "DOCTOR", IsActive = true, EntryDate= DateTime.Now },
            };
        }

        public static IEnumerable<User> Users(ApplicationDbContext appContext)
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
                .RuleFor(u => u.RoleId, f => roleIds[f.Random.Int(0, roleIds.Count - 1)]) // Gắn roleId từ 1-4
                .RuleFor(u => u.DateOfBirth, f => f.Date.Past(40, DateTime.Now.AddYears(-18)))
                .RuleFor(u => u.Gender, f => f.PickRandom<Gender>())
                .RuleFor(u => u.EntryDate, f => DateTime.Now);

            return faker.Generate(50);
        }

        public static IEnumerable<Appointment> Appointments(IServiceProvider serviceProvider)
        {
            var appContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
            var doctors = appContext.Doctors.ToList();
            var patients = appContext.Patients.ToList();
            var medications = appContext.Medications.ToList(); // Lấy danh sách thuốc

            var faker = new Faker<Appointment>()
                .RuleFor(a => a.AppointmentDate, f => f.Date.Future())
                .RuleFor(a => a.Reason, f => f.PickRandom<AppointmentReason>())
                .RuleFor(a => a.OtherReason, f => f.Address.FullAddress())
                .RuleFor(a => a.PatientId, f => patients[f.Random.Int(0, patients.Count - 1)].Id)
                .RuleFor(a => a.DoctorId, f => doctors[f.Random.Int(0, doctors.Count - 1)].Id)
                .RuleFor(a => a.Status, f => f.PickRandom<AppointmentStatus>())
                .RuleFor(a => a.EntryDate, f => DateTime.Now)
                .RuleFor(a => a.MedicalReports, f => GenerateMedicalReports(f, medications))
                .RuleFor(u => u.EntryDate, f => DateTime.Now);


            return faker.Generate(15);
        }

        // Generate MedicalReport
        private static List<MedicalReport> GenerateMedicalReports(Faker faker, List<Medication> medications)
        {
            return new Faker<MedicalReport>()
                .RuleFor(mr => mr.ReportDate, f => f.Date.Past())
                .RuleFor(mr => mr.Findings, f => f.Lorem.Sentence(10))
                .RuleFor(mr => mr.Recommendations, f => f.Lorem.Sentence(5))
                .RuleFor(mr => mr.Diagnosis, f => f.Lorem.Sentence(7))
                .RuleFor(mr => mr.DoctorNotes, f => f.Lorem.Sentence(8))
                .RuleFor(mr => mr.PrescriptionDetails, f => GeneratePrescriptionDetails(f, medications))
                .RuleFor(u => u.EntryDate, f => DateTime.Now)
                .Generate(faker.Random.Int(1, 3)); 
        }

        // Generate PrescriptionDetail
        private static List<PrescriptionDetail> GeneratePrescriptionDetails(Faker faker, List<Medication> medications)
        {
            return new Faker<PrescriptionDetail>()
                .RuleFor(pd => pd.Quantity, f => f.Random.Int(1, 5))
                .RuleFor(pd => pd.Dosage, f => $"{f.Random.Int(1, 3)} lần/ngày")
                .RuleFor(pd => pd.Instructions, f => f.Lorem.Sentence(5))
                .RuleFor(pd => pd.MedicationId, f => medications[f.Random.Int(0, medications.Count - 1)].Id)
                .RuleFor(u => u.EntryDate, f => DateTime.Now)

                .Generate(faker.Random.Int(1, 5));
        }

        public static IEnumerable<Medication> Medications()
        {
            var faker = new Faker<Medication>()
                .RuleFor(m => m.Name, f => f.Commerce.ProductName())
                .RuleFor(m => m.Description, f => f.Lorem.Sentence(10))
                .RuleFor(m => m.Price, f => Math.Round(f.Random.Double(5.0, 100.0), 2)) 
                .RuleFor(m => m.StockQuantity, f => f.Random.Int(0, 500)) 
                .RuleFor(m => m.Manufacturer, f => f.Company.CompanyName())
                .RuleFor(m => m.EntryDate, f => DateTime.Now);

            return faker.Generate(60);
        }

        public static async Task WorkWeeks(ApplicationDbContext appContext)
        {
            // just suport celander for doctor
            var doctors = appContext.Doctors.ToList();
            var faker = new Faker<WorkWeek>()
                .RuleFor(w => w.UserId, f => doctors[f.Random.Int(0, doctors.Count - 1)].UserId)
                .RuleFor(w => w.WeekStartDate, f =>
                {
                    var currentDate = DateTime.Today;
                    int daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)currentDate.DayOfWeek + 7) % 7; // Tính số ngày đến thứ Hai của tuần sau
                    var nextMonday = currentDate.AddDays(daysUntilNextMonday); // Thứ Hai của tuần tới

                    return nextMonday;
                })
                .RuleFor(w => w.WeekEndDate, (f, w) => w.WeekStartDate.AddDays(6)) // End date sẽ là ngày Chủ Nhật (7 ngày sau)
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
