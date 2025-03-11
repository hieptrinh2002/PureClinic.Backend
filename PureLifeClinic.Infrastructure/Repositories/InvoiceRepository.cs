﻿using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;

namespace PureLifeClinic.Infrastructure.Repositories
{
    public class InvoiceRepository : BaseRepository<Invoice>, IInvoiceRepository
    {
        public InvoiceRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public Task<Invoice?> GetInvoiceByAppoinmentId(int appoinmentId)
        {
            return _dbContext.Invoices.Include(i => i.Appointment)
                .FirstOrDefaultAsync(i => i.AppointmentId == appoinmentId);
        }
    }
}