using AutoMapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

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
    }
}
