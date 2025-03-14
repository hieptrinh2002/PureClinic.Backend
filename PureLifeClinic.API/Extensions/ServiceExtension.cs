using CloudinaryDotNet;
using Microsoft.Extensions.Options;
using PureLifeClinic.API.ActionFilters;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using PureLifeClinic.Core.Interfaces.IServices.IFileGenarator;
using PureLifeClinic.Core.Services;
using PureLifeClinic.Core.Services.FileGenerator;
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
            services.AddTransient<ITokenService, TokenService>();
            services.AddTransient<IMailService, MailService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddTransient<IRefreshTokenService, RefreshTokenService>();
            services.AddTransient<IWorkWeekScheduleService, WorkWeekScheduleService>();
            services.AddTransient<IAppointmentService, AppointmentService>();
            services.AddTransient<IMedicalReportService, MedicalReportService>();
            services.AddTransient<IMedicineService, MedicineService>();
            services.AddTransient<IPrescriptionDetailService, PrescriptionDetailService>();
            services.AddTransient<IMedicalFileService, MedicalFileService>();
            services.AddTransient<IInvoiceService, InvoiceService>();
            #endregion

            #region FileGenaratorService  
            services.AddScoped<IFileGenerator<InvoiceFileCreateViewModel>, InvoiceGenerator>();
            #endregion

            #region Validators  
            services.AddScoped<IValidationService, ValidationService>();
            #endregion


            services.AddScoped<ValidateInputViewModelFilter>();



            #region RabbitMQ
            //services.AddSingleton<IRabbitMQConnection>(new RabbitMQConnection());
            #endregion

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
