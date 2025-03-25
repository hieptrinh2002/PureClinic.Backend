using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.API.Helpers
{
    public class MailHelper
    {
        public static string ReadAndProcessHtmlTemplate(string filePath, string activationLink, string userName , string resetPasswordLink = "")
        {
            var htmlContent = File.ReadAllText(filePath);
            htmlContent = htmlContent
                .Replace("{{UserName}}", userName)
                .Replace("{{ActivationLink}}", activationLink)
                .Replace("{{ResetPasswordLink}}", resetPasswordLink)
                .Replace("{{Year}}", DateTime.Now.Year.ToString())
                .Replace("{{UserEmail}}", "johndoe@example.com");

            return htmlContent;
        }

        public static string GenerateConfirmationLink(string userEmail, string ClientUrl, string token)
        {
            var _token = Uri.EscapeDataString(token);
            var email = Uri.EscapeDataString(userEmail);
            return $"{ClientUrl}?token={_token}&email={email}";
        }
    }
}
