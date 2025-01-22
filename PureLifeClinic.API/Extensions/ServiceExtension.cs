using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using PureLifeClinic.Core.Services;
using PureLifeClinic.Infrastructure.Repositories;

namespace PureLifeClinic.API.Extensions
{
    public static class ServiceExtension
    {
        public static IServiceCollection RegisterService(this IServiceCollection services)
        {
            #region Services
            services.AddSingleton<IUserContext, UserContext>();
            services.AddSingleton<ICloudinaryService, CloudinaryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IRoleService, RoleService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IDoctorService, DoctorService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddTransient<IMailService, MailService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            services.AddTransient<IWorkWeekScheduleService, WorkWeekScheduleService>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<IMedicalReportService, MedicalReportService>();
            services.AddTransient<IMedicineService, MedicineService>();

            //Cloudinary
            services.AddSingleton<Cloudinary>(serviceProvider =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<AppSettings>>().Value;
                var account = new Account(
                    options.CloudinaryConfig.CloudName,
                    options.CloudinaryConfig.ApiKey,
                    options.CloudinaryConfig.ApiSecret
                );
                return new Cloudinary(account);
            });
            #endregion

            #region Repositories
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IRoleRepository, RoleRepository>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IAuthRepository, AuthRepository>();
            services.AddTransient<IRefreshTokenRepository, RefreshTokenRepository>();
            services.AddTransient<IWorkWeekScheduleRepository, WorkWeekScheduleRepository>();
            services.AddTransient<IAppointmentRepository, AppointmentRepository>();

            #endregion

            services.AddTransient<IUnitOfWork, UnitOfWork>();
            return services;
        }
    }
}
