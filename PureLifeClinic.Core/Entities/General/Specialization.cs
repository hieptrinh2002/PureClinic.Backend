using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class Specialization : Base<int>
    {
        [Required]
        [MaxLength(255)]
        public string SpecializationName { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        // Display order
        public int DisplayOrder { get; set; }

        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }
}
