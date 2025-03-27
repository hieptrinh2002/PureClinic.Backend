using PureLifeClinic.Application.BusinessObjects.ErrorViewModels;

namespace PureLifeClinic.Application.BusinessObjects.ResponseViewModels
{
    public class ResponseViewModel<T>
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public ErrorViewModel? Error { get; set; }
    }

    public class ResponseViewModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public ErrorViewModel? Error { get; set; }
    }
}
