﻿using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Interfaces.IRepositories
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthRepository Auth { get; }
        IProductRepository Products { get; }
        IRefreshTokenRepository RefreshTokens { get; }
        IRoleRepository Roles { get; }
        IUserRepository Users { get; }
        IWorkWeekScheduleRepository WorkWeeks { get; }
        IAppointmentRepository Appointments { get; }
        IPatientRepository Patients { get; }
        IDoctorRepository Doctors { get; }
        IMedicalReportRepository MedicalReports { get; }
        IMedicineRepository Medicines { get; }
        IPrescriptionDetailRepository PrescriptionDetails { get; }
        IMedicalFileRepository MedicalFiles { get; }
        IPermissionRepository Permissions { get;  }
        IInvoiceRepository Invoices { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
        new void Dispose();
    } 
}
