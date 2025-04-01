using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.RoleViewModels.Request
{
    public class RoleCreateViewModel
    {
        public string? Code { get; set; }

        public string? Name { get; set; }

        public bool IsActive { get; set; }
    }
}
