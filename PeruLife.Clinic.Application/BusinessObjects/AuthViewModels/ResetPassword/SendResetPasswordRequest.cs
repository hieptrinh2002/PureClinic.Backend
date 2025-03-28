﻿using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.AuthViewModels.ResetPassword
{
    public class SendResetPasswordRequest
    {
        [Required, StringLength(20, MinimumLength = 2)]
        public string Email { get; set; }

        [Required, StringLength(50, MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        public string Token { get; set; }
    }
}
