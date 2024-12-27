using Project.Core.Entities.General;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PureLifeClinic.Core.Entities.General
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Token { get; set; }
        public string? AccessTokenId { get; set; }

        public DateTime ExpireOn { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpireOn;

        // the time when the token is created
        public DateTime CreateOn { get; set; }

        // the time when the token is revoked/canceled
        public DateTime? RevokedOn { get; set; }

        // we have two conditions for token to be active"IsActive" => was not revoked and not IsExpired
        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}
