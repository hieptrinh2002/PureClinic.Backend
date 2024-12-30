﻿using PureLifeClinic.Core.Entities.General;

namespace PureLifeClinic.Core.Entities.Business
{
    public class AuthResultViewModel
    {
        public string? AccessToken { get; set; }

        public RefreshTokenViewModel RefreshToken { get; set; }
    }
}
