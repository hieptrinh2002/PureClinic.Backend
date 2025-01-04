using PureLifeClinic.Core.Entities.Business;

namespace PureLifeClinic.API.Helpers
{
    public class MailHelper
    {
        public static string ReadAndProcessHtmlTemplate(string filePath, string activationLink, string userName , string resetPasswordLink = "")
        {
            // Đọc nội dung của file HTML
            var htmlContent = File.ReadAllText(filePath);

            // Thay thế các placeholder trong file HTML
            htmlContent = htmlContent
                .Replace("{{UserName}}", userName)
                .Replace("{{ActivationLink}}", activationLink)
                .Replace("{{ResetPasswordLink}}", resetPasswordLink)
                .Replace("{{Year}}", DateTime.Now.Year.ToString())
                .Replace("{{UserEmail}}", "johndoe@example.com");

            return htmlContent;
        }

        public static string GenerateConfirmationLink(string userEmail, string ClientUrl, string activationToken)
        {
            var token = Uri.EscapeDataString(activationToken);
            var email = Uri.EscapeDataString(userEmail);
            return $"{ClientUrl}?token={token}&email={email}";
        }

    }
}
