using CloudinaryDotNet;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using PeruLife.Clinic.Application.Services;
using PureLifeClinic.API.ActionFilters;
using PureLifeClinic.Application.BusinessObjects.InvoiceViewModels.Request;
using PureLifeClinic.Application.Interfaces;
using PureLifeClinic.Application.Interfaces.IBackgroundJobs;
using PureLifeClinic.Application.Interfaces.IQueue;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Application.Interfaces.IServices.IFileGenarator;
using PureLifeClinic.Application.Interfaces.IServices.INotification;
using PureLifeClinic.Application.Services;
using PureLifeClinic.Application.Services.FileGenerator;
using PureLifeClinic.Application.Services.Queues;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.BackgroundServices;
using PureLifeClinic.Infrastructure.BackgroundServices.Jobs;
using PureLifeClinic.Infrastructure.ExternalServices;
using PureLifeClinic.Infrastructure.ExternalServices.Email;
using PureLifeClinic.Infrastructure.Persistence.Repositories;
using PureLifeClinic.Infrastructure.SignalR;

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
            services.AddTransient<IMailService, EmailService>();
            services.AddSingleton<IEmailTemplateService, EmailTemplateService>();
            services.AddScoped<IPermissionService, PermissionService>();
            services.AddScoped<IRefreshTokenService, RefreshTokenService>();
            services.AddScoped<IWorkWeekScheduleService, WorkWeekScheduleService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IMedicalReportService, MedicalReportService>();
            services.AddScoped<ICounterService, CounterService>();
            services.AddScoped<IMedicineService, MedicineService>();
            services.AddScoped<IPrescriptionDetailService, PrescriptionDetailService>();
            services.AddScoped<IMedicalFileService, MedicalFileService>();
            services.AddScoped<IInvoiceService, InvoiceService>();
            services.AddScoped<IPatientService, PatientService>();  
            services.AddScoped<ILabResultService, LabResultService>();
            services.AddTransient<IWaitingQueueService, WaitingQueueService>();
            services.AddTransient<IConsultationQueueService, ConsultationQueueService>();
            services.AddTransient<IExaminationQueueService, ExaminationQueueService>();
            services.AddSingleton<IRedisQueueService, RedisQueueService>();

            #endregion

            #region FileGenaratorService  
            services.AddScoped<IFileGenerator<InvoiceFileCreateViewModel>, InvoiceGenerator>();
            #endregion

            #region Validators  
            services.AddScoped<IValidationService, ValidationService>();
            #endregion

            services.AddScoped<ValidateInputViewModelFilter>();
            services.AddScoped<IBackgroundJobService, BackgroundJobService>();
            services.AddScoped<IRecurringJobService, RecurringJobService>();
            services.AddScoped<IAppointmentReminderJob, AppointmentReminderJob>();
            services.AddScoped<IAutoUpdateLateAppointmentJob, AutoUpdateLateAppointmentJob>();

            //services.AddScoped<IReportGenerationJob, ReportGenerationJob>();

            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IHubConnectionService, HubConnectionService>();

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