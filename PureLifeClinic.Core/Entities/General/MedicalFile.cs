using PureLifeClinic.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class MedicalFile : Base<int>
    {
        public FileType FileType { get; set; }

        public string FilePath { get; set; }= String.Empty;

        public float FileSize { get; set; } = 0;

        public string FileName { get; set; } = String.Empty;

        // Navigation property
        public int MedicalReportId { get; set; }
        [ForeignKey(nameof(MedicalReportId))]
        public MedicalReport? MedicalReport { get; set; }

        public int? AppointmentHealthServiceId { get; set; }
        [ForeignKey(nameof(AppointmentHealthServiceId))]
        public AppointmentHealthService? AppointmentHealthService { get; set; }
    }
}