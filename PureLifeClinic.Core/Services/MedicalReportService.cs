using AutoMapper;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using System.Linq.Expressions;
using System.Threading;
using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Core.Services
{
    public class MedicalReportService : BaseService<MedicalReport, MedicalReportViewModel>, IMedicalReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        public MedicalReportService(IMapper mapper, IUserContext userContext, IUnitOfWork unitOfWork) : base(mapper, unitOfWork.MedicalReports)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _userContext = userContext;
        }

        public async Task<MedicalReportViewModel> Create(MedicalReportCreateViewModel model, CancellationToken cancellationToken)
        {
            var medicalReport = _mapper.Map<MedicalReport>(model);
            medicalReport.EntryDate = DateTime.Now;
            medicalReport.EntryBy = Convert.ToInt32(_userContext.UserId);
            medicalReport.ReportDate = DateTime.Now;
            var result = await _unitOfWork.MedicalReports.Create(medicalReport, cancellationToken);
            return _mapper.Map<MedicalReportViewModel>(result);
        }

        public async Task Delete(int id, CancellationToken cancellationToken)
        {
            var entity = await _unitOfWork.MedicalReports.GetById(id, cancellationToken);
            await _unitOfWork.MedicalReports.Delete(entity, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateMedicalReportAsync(MedicalReportUpdateViewModel model, CancellationToken cancellationToken)
        {
            var medicalReport = (await _unitOfWork.MedicalReports.GetById(model.Id, cancellationToken))
                ?? throw new KeyNotFoundException($"Medical report with ID {model.Id} not found.");
            _mapper.Map(model, medicalReport);
            medicalReport.UpdatedDate = DateTime.Now;
            medicalReport.UpdatedBy = Convert.ToInt32(_userContext.UserId);
            await _unitOfWork.MedicalReports.Update(medicalReport, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        public async Task<ResponseViewModel<IEnumerable<MedicalReportViewModel>>> GetByPatientId(int patientId, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.Patients.GetById(patientId, cancellationToken) ?? throw new Exception("Patient not found");

            var medicalReports = await _unitOfWork.MedicalReports.GetMedicalReportByPatientId(patient.Id, cancellationToken);

            //var result = medicalReports.Select(m => new MedicalReportViewModel
            //{
            //    Id = m.Id,
            //    EntryDate = m.EntryDate,
            //    AppointmentId = m.AppointmentId,
            //    ReportDate = m.ReportDate,
            //    Findings = m.Findings,
            //    Recommendations = m.Recommendations,
            //    Diagnosis = m.Diagnosis,
            //    DoctorNotes = m.DoctorNotes,
            //    MedicalFiles = m.MedicalFiles?.Select(f => f.FilePath).ToList() ?? new List<string>(),
            //    PrescriptionDetails = m.PrescriptionDetails?.Select(pd => new PrescriptionDetailViewModel
            //    {
            //        Id = pd.Id,
            //        Quantity = pd.Quantity,
            //        Dosage = pd.Dosage,
            //        Instructions = pd.Instructions,
            //        MedicalReportId = pd.MedicalReportId,
            //        Medicine = new MedicineViewModel
            //        {
            //            Id = pd.Medicine.Id,
            //            Code = pd.Medicine.Code,
            //            Name = pd.Medicine.Name,
            //            Price = pd.Medicine.Price,
            //            Quantity = pd.Medicine.Quantity,
            //            Description = pd.Medicine.Description,
            //            IsActive = pd.Medicine.IsActive
            //        }
            //    }).ToList()
            //});

            return new ResponseViewModel<IEnumerable<MedicalReportViewModel>>
            {
                Data = _mapper.Map<IEnumerable<MedicalReportViewModel>>(medicalReports),
                Message = "Get medical report susscessful",
                Success = true,
            };
        }

        public async Task<PaginatedDataViewModel<MedicalReportViewModel>> GetByPatientId(int patientId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var includeList = new List<Expression<Func<MedicalReport, object>>> { x => x.Appointment };

            var filters = new List<ExpressionFilter>
            {
                new ExpressionFilter
                {
                    PropertyName = "Appointment.PatientId",
                    Value = patientId,
                    Comparison = Comparison.Equal
                }
            };
            var paginatedData = await _unitOfWork.MedicalReports.GetPaginatedData(includeList, pageNumber, pageSize, cancellationToken, filters);

            var paginatedDataViewModel = new PaginatedDataViewModel<MedicalReportViewModel>(_mapper.Map<IEnumerable<MedicalReportViewModel>>(paginatedData.Data), paginatedData.TotalCount);
            return paginatedDataViewModel;
        }

        public async Task<ResponseViewModel<MedicalReportViewModel>> GetById(int id, CancellationToken cancellationToken)
        {
            var patient = await _unitOfWork.MedicalReports.GetById(id, cancellationToken) ?? throw new Exception("Medical report not found");
            var includeExpressions = new List<Func<IQueryable<MedicalReport>, IIncludableQueryable<MedicalReport, object>>>
            {
                query => query.Include(m => m.PrescriptionDetails).ThenInclude(p => p.Medicine),
                query => query.Include(a => a.MedicalFiles)
            };


            var result = await _unitOfWork.MedicalReports.GetById(id, includeExpressions, cancellationToken);

            return new ResponseViewModel<MedicalReportViewModel>
            {
                Success = true,
                Message = "Get medical report by id scucessfully",
                Data = _mapper.Map<MedicalReportViewModel>(result)
            };
        }
    }
}
 