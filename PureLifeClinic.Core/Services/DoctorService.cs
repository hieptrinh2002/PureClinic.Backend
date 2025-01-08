using AutoMapper;
using PureLifeClinic.Core.Common;
using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using PureLifeClinic.Core.Interfaces.IRepositories;
using PureLifeClinic.Core.Interfaces.IServices;
using System.Linq.Expressions;

namespace PureLifeClinic.Core.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;   
        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DoctorViewModel>> GetAll(CancellationToken cancellationToken)
        {
            var entities = await _unitOfWork.Users.GetAllDoctor(cancellationToken);
            var doctorViewModels = _mapper.Map<IEnumerable<DoctorViewModel>>(entities);
            return doctorViewModels;
        }

        public async Task<DoctorViewModel> GetById(int id, CancellationToken cancellationToken)
        {
            var resut = await _unitOfWork.Users.GetDoctorById(id, cancellationToken);
            return _mapper.Map<DoctorViewModel>(resut);
        }

        public async Task<PaginatedDataViewModel<DoctorViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var includeList = new List<Expression<Func<User, object>>> { x => x.Role, x=> x.Doctor};

            var filters = new List<ExpressionFilter>
            {
                new ExpressionFilter
                {
                    PropertyName = "Doctor",
                    Value = null,
                    Comparison = Comparison.NotEqual 
                }
            };
            var paginatedData = await _unitOfWork.Users.GetPaginatedData(includeList, pageNumber, pageSize, cancellationToken, filters);

            var paginatedDataViewModel = new PaginatedDataViewModel<DoctorViewModel>(_mapper.Map<IEnumerable<DoctorViewModel>>(paginatedData.Data), paginatedData.TotalCount);
            return paginatedDataViewModel;
        }

        public Task<ResponseViewModel> Update(DoctorUpdateViewModel model, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
