using AutoMapper;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;

namespace PureLifeClinic.Core.Services
{
    public class PrescriptionDetailService : BaseService<PrescriptionDetail, PrescriptionDetailViewModel>, IPrescriptionDetailService
    {
        private readonly IUnitOfWork _unitOfWork;   
        private readonly IMapper _mapper;   
        public PrescriptionDetailService(IMapper mapper, IUnitOfWork unitOfWork) : base(mapper, unitOfWork.PrescriptionDetails)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
    }
}
