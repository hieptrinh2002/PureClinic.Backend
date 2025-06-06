﻿using PureLifeClinic.Core.Enums;

namespace PureLifeClinic.Application.BusinessObjects.UserViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Role { get; set; }

        public string? Address { get; set; }
        public Gender? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
    }
}
