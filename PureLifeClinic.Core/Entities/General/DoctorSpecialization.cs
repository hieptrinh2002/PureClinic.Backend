using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class DoctorSpecialization : Base<int>
    {
        [Required]
        [MaxLength(255)]
        public string SpecializationName { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        // Display order
        public int DisplayOrder { get; set; }

        // Navigation property: Nếu mô hình là Many-to-Many giữa Doctor và Specialization,
        // bạn có thể thêm collection này để dễ dàng truy xuất các bác sĩ có chuyên môn này.
        public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    }

}
