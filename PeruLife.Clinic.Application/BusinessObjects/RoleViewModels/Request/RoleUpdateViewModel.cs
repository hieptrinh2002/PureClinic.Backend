using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.RoleViewModels.Request
{
    public class RoleUpdateViewModel
    {
        public int Id { get; set; }

        public string? Code { get; set; }

        public string? Name { get; set; }

        public bool IsActive { get; set; }
    }
}
