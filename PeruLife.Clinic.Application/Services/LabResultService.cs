using PureLifeClinic.Application.BusinessObjects.LabResultViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Enums;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services
{
    public class LabResultService : ILabResultService
    {
        private readonly IUnitOfWork _unitOfWork;

        public LabResultService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<LabResult>> FilterAsync(int patientId, string? testType, LabTestStatus? status, DateTime? from, DateTime? to)
        {
            var query = await _unitOfWork.LabResults.QueryAsync(patientId);

            if (!string.IsNullOrEmpty(testType))
                query = query.Where(r => r.TestType.Contains(testType));

            if (status.HasValue)
                query = query.Where(r => r.TestStatus == status);

            if (from.HasValue)
                query = query.Where(r => r.TestDate >= from);

            if (to.HasValue)
                query = query.Where(r => r.TestDate <= to);

            return query.ToList();
        }

        public async Task<LabResult?> GetByIdAsync(int patientId, int id)
        {
            return await _unitOfWork.LabResults.GetByIdAsync(patientId, id);
        }

        public async Task<bool> UpdateAsync(int patientId, int id, LabResultUpdateViewModel dto)
        {
            var labResult = await _unitOfWork.LabResults.GetByIdAsync(patientId, id);
            if (labResult == null) return false;

            labResult.Results = dto.Results.Select(r => new TestResult
            {
                Name = r.Name,
                Value = r.Value,
                NormalRange = r.NormalRange,
                Status = r.Status
            }).ToList();

            labResult.TestStatus = dto.Status;

            _unitOfWork.LabResults.Update(labResult);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
