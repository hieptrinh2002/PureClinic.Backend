using Microsoft.AspNetCore.Identity;
using PureLifeClinic.Core.Enums;
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
        public DateTime? DateOfBirth { get; set; }
        public string? ImagePath { get; set; }

        public bool IsLockPermission { get; set; } = false; 
        public int? Age
        {
            get
            {
                if (DateOfBirth == null)
                    return 0;
                return DateTime.Now.Year - DateOfBirth?.Year;
            }
        }

        [ForeignKey(nameof(RoleId))]
        public Role Role { get; set; }
        public virtual Doctor? Doctor { get; set; }
        public virtual Patient? Patient { get; set; }
        public List<UserPermission> Permissions { get; private set; } = new();
        public virtual ICollection<WorkWeek> WorkWeeks { get; set; } = new List<WorkWeek>();

        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}