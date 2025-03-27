namespace PureLifeClinic.Application.BusinessObjects.EmailViewModels
{
    public class EmailActivationViewModel
    {
        public int UserId { get; set; }

        public string ActivationToken { get; set; }

        public string ActivationUrl { get; set; }
    }
}
