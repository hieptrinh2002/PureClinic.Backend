using Microsoft.AspNetCore.Identity;
using System.Text.RegularExpressions;

namespace PureLifeClinic.Infrastructure.Identity.Validators
{
    public class CustomPasswordValidator<TUser> : PasswordValidator<TUser> where TUser : class
    {
        public override async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string? password)
        {
            var result = await base.ValidateAsync(manager, user, password);
            var errors = new List<IdentityError>(result.Succeeded ? new List<IdentityError>() : result.Errors);
            errors.AddRange(Validate(password));
            return errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray());
        }

        public static List<IdentityError> Validate(string? password)
        {
            var errors = new List<IdentityError>();

            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordRequired",
                    Description = "Password is required."
                });
                return errors;
            }

            if (password.Length < 8)
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordTooShort",
                    Description = "Password must be at least 8 characters."
                });
            }

            if (password.Length > 50)
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordTooLong",
                    Description = "Password must not exceed 50 characters."
                });
            }

            if (!Regex.IsMatch(password, "[A-Z]"))
            {
                errors.Add(new IdentityError
                {
                    Code = "MissingUppercase",
                    Description = "Password must contain at least one uppercase letter."
                });
            }

            if (!Regex.IsMatch(password, "[a-z]"))
            {
                errors.Add(new IdentityError
                {
                    Code = "MissingLowercase",
                    Description = "Password must contain at least one lowercase letter."
                });
            }

            if (!Regex.IsMatch(password, @"\d"))
            {
                errors.Add(new IdentityError
                {
                    Code = "MissingDigit",
                    Description = "Password must contain at least one number."
                });
            }

            if (!Regex.IsMatch(password, @"[\@\$\!\%\*\?\&\#\^\.\-_]"))
            {
                errors.Add(new IdentityError
                {
                    Code = "MissingSpecialChar",
                    Description = "Password must contain at least one special character (e.g., @, $, !, %, *, ?, &, #)."
                });
            }

            if (Regex.IsMatch(password.ToLower(), @"hospital|admin|123456|qwerty"))
            {
                errors.Add(new IdentityError
                {
                    Code = "CommonWeakWords",
                    Description = "Password cannot contain common weak words like 'hospital', 'admin', etc."
                });
            }

            if (password.ToLower().Contains("password"))
            {
                errors.Add(new IdentityError
                {
                    Code = "ContainsPasswordWord",
                    Description = "Password cannot contain the word 'password'."
                });
            }

            return errors;
        }
    }
}
