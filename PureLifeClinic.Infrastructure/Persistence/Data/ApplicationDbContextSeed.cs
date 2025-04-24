using Bogus;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Infrastructure.Persistence.Data.SeedData;

namespace PureLifeClinic.Infrastructure.Persistence.Data
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
            var roleManager = serviceProvider.GetRequiredService<RoleManager<Role>>();

            try
            {
                // Adding Roles
                if (!appContext.Roles.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    appContext.Roles.AddRange(RoleSeed.SeedRoleData());
                    await appContext.SaveChangesAsync();
                    transaction.Commit();
                }

                //  Adding RoleClaims
                if (!appContext.RoleClaims.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    await RoleClaimSeed.SeedRoleClaims(appContext);
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
                    var users = UserSeed.SeedUserData(appContext).ToList();

                    // handle parallel
                    int maxConcurrency = Environment.ProcessorCount * 2;
                    using var semaphore = new SemaphoreSlim(maxConcurrency);
                    var roleMap = roles.ToDictionary(r => r.Id, r => r); // tra cứu nhanh
                    var tasks = users.Select(async user =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            if (!roleMap.TryGetValue(user.RoleId, out var role))
                                return;

                            var defaultPassword = GenerateStrongPassword();
                            var result = await UserManager.CreateAsync(user, defaultPassword);

                            if (!result.Succeeded)
                            {
                                Console.WriteLine($"Failed to create user: {user.UserName}, Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                                return;
                            }

                            await UserManager.AddToRoleAsync(user, role.NormalizedName);

                            switch (role.NormalizedName)
                            {
                                case "DOCTOR":
                                    appContext.Doctors.Add(DoctorSeed.SeedOneDoctorsData(user.Id));
                                    break;
                                case "PATIENT":
                                    appContext.Patients.Add(PatientSeed.SeedPatientsData(user.Id));
                                    break;
                                case "EMPLOYEE":
                                case "ADMIN":
                                    break;
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    }).ToList();

                    await Task.WhenAll(tasks);

                    // Save all at once if needed
                    await appContext.SaveChangesAsync();
                }

                // Adding Medication
                if (!appContext.Medicines.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    var medicineData = MedicineSeed.SeedMedicineData();
                    appContext.Medicines.AddRange(medicineData);
                    await appContext.SaveChangesAsync();
                    transaction.Commit();
                }

                // Adding Schedule
                if (!appContext.WorkWeeks.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    await WorkWeekSeed.SeedWorkWeekData(appContext);
                    transaction.Commit();
                }

                // Adding appoinments
                if (!appContext.Appointments.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    appContext.Appointments.AddRange(AppointmentSeed.SeedAppointmentData(serviceProvider));
                    await appContext.SaveChangesAsync();
                    transaction.Commit();
                }

                // add HealthServices   
                if (!appContext.HealthServices.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    appContext.HealthServices.AddRange(HealthServiceSeed.SeedHealthServicesData(10));
                    await appContext.SaveChangesAsync();
                    transaction.Commit();
                    foreach (var service in appContext.HealthServices)
                    {
                        service.HierarchyPath = $"{service.Id}/";
                    }
                    await appContext.SaveChangesAsync();
                }

                // add Rooms
                if (!appContext.Rooms.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    appContext.RoomTypes.AddRange(RoomTypeSeed.SeedRoomTypeData());
                    await appContext.SaveChangesAsync();
                    transaction.Commit();
                }

                // add AppointmentHealthServices    
                if (!appContext.AppointmentHealthServices.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    var appointments = appContext.Appointments.ToList();
                    var healthServices = appContext.HealthServices.ToList();
                    var rooms = appContext.Rooms.ToList();  
                    if (!appointments.Any() || !healthServices.Any() || !rooms.Any())
                        return;

                    var appointmentHealthServices = AppointmentHealthServiceSeed.SeedAppointmentHealthServicesData(appointments, healthServices, rooms);
                    appContext.AppointmentHealthServices.AddRange(appointmentHealthServices);
                    await appContext.SaveChangesAsync();
                    transaction.Commit();
                }

                //  Adding RoleClaims
                if (!appContext.Counters.Any())
                {
                    using var transaction = appContext.Database.BeginTransaction();
                    await CounterSeed.SeedCounterData(appContext);
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
    }
}
