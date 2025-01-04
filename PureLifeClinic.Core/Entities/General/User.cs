using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class User : IdentityUser<int>
    {
        [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
        public string FullName { get; set; }
        public bool IsActive { get; set; }
        [Required]
        public int RoleId { get; set; }
        public int? EntryBy { get; set; }
        public DateTime? EntryDate { get; set; }
        public int? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string? Address { get; set; } 
        public Gender? Gender { get; set; } 
        public DateTime? DateOfBirth { get; set; } // Ngày sinh.

        [ForeignKey(nameof(RoleId))]
        public virtual Role Role { get; set; }
        public virtual Doctor? Doctor { get; set; }
        public virtual Patient? Patient { get; set; }

        public virtual ICollection<WorkWeek> WorkWeeks { get; set; } = new List<WorkWeek>();

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }

    public enum Gender
    {
        Female,
        Male
    }
}