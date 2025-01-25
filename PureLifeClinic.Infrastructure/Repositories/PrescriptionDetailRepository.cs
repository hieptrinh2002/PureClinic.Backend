﻿using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Infrastructure.Data;

namespace PureLifeClinic.Infrastructure.Repositories
{
    public class PrescriptionDetailRepository : BaseRepository<PrescriptionDetail>, IPrescriptionDetailRepository
    {
        public PrescriptionDetailRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<PrescriptionDetail>> GetByMedicalReportId(int medicalReportId, CancellationToken cancellationToken)
        {
            var result = await _dbContext.PrescriptionDetails.Where( p => p.MedicalReportId == medicalReportId ).ToListAsync(cancellationToken);
            return result;
        }
    }
}
