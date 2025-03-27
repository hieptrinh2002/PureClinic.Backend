﻿using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.PatientsViewModels
{
    public class PatientViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
