using AutoMapper;
using PureLifeClinic.Application.BusinessObjects.PrescriptionDetailViewModels;
using PureLifeClinic.Application.Interfaces.IServices;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;

namespace PureLifeClinic.Application.Services
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
