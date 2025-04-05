using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.WaitingQueues.Request
{
    public class AddToExaminationQueueRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public ConsultationType Type { get; set; }
    }
}
