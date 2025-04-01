namespace PureLifeClinic.Application.BusinessObjects.RoleViewModels.Response
{
    public class RoleViewModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
    }
}
