using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels.Token
{
    public class RefreshTokenCreateViewModel
    {
        public int UserId { get; set; }

        public string Token { get; set; }

        public string? AccessTokenId { get; set; }

        public DateTime ExpireOn { get; set; }

        // the time when the token is created
        public DateTime CreateOn { get; set; } = DateTime.UtcNow;

        // the time when the token is revoked/canceled
        public DateTime? RevokedOn { get; set; }
    }
}
