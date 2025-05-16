using PureLifeClinic.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class MedicalFile : Base<int>
    {
        public FileType FileType { get; set; }

        public string FilePath { get; set; } = string.Empty;

        public string FilePathPublicId { get; set; } = string.Empty;

        public float FileSize { get; set; } = 0;

        public string FileName { get; set; } = string.Empty;

        // Navigation property
        public int MedicalReportId { get; set; }
        [ForeignKey(nameof(MedicalReportId))]
        public MedicalReport? MedicalReport { get; set; }

        public int? AppointmentHealthServiceId { get; set; }
        [ForeignKey(nameof(AppointmentHealthServiceId))]
        public AppointmentHealthService? AppointmentHealthService { get; set; }
    }
}