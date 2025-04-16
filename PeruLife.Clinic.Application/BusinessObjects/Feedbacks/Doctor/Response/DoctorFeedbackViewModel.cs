﻿namespace PureLifeClinic.Application.BusinessObjects.Feedbacks.Doctor.Response
{
    public class DoctorFeedbackViewModel
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
