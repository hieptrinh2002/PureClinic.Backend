using PureLifeClinic.Core.Entities.Business;
using PureLifeClinic.Core.Entities.General;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PureLifeClinic.Core.Interfaces.IServices
{
    public interface IDoctorService
    {
        new Task<IEnumerable<DoctorViewModel>> GetAll(CancellationToken cancellationToken);
        new Task<PaginatedDataViewModel<DoctorViewModel>> GetPaginatedData(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<DoctorViewModel> GetById(int id, CancellationToken cancellationToken);
        Task<ResponseViewModel> Update(DoctorUpdateViewModel model, CancellationToken cancellationToken);
    }
}
