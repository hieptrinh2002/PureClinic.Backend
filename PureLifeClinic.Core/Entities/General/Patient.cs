﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class Patient: Base<int>
    {
        [StringLength(1000)]
        public string? Notes { get; set; } = string.Empty;

        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>(); 

        public int UserId { get; set; }

        [Required, ForeignKey(nameof(UserId))]
        public User User { get; set; }

        public int? PrimaryDoctorId { get; set; }

        [ForeignKey(nameof(PrimaryDoctorId))]
        public  Doctor? PrimaryDoctor { get; set; }

        // new 
        public PatientStatus Status { get; set; } = PatientStatus.New;
        public ICollection<LabResult>? LabResults { get; set; }
        public ICollection<MedicationInteraction>? Interactions { get; set; }
        public ICollection<Allergy>? Allergies { get; set; }
    }
    public enum PatientStatus
    {
        New,
        UnderTreatment, 
        Stable,        
        Discharged,     
        Referred        
    }
}