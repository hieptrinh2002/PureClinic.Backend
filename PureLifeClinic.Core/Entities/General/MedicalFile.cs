using PureLifeClinic.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class MedicalFile : Base<int>
    {
        public FileType FileType { get; set; }
        public string FilePath { get; set; }
        public float? FileSize { get; set; }

        public string? FileName { get; set; }

        // Navigation property
        public int MedicalReportId { get; set; }

        [ForeignKey(nameof(MedicalReportId))]

        public MedicalReport? MedicalReport { get; set; }
    }
}
