﻿using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Application.BusinessObjects.RoleViewModels
{
    public class RoleCreateViewModel
    {
        [Required, StringLength(maximumLength: 10, MinimumLength = 2)]
        public string? Code { get; set; }

        [Required, StringLength(100, MinimumLength = 2)]
        public string? Name { get; set; }

        public bool IsActive { get; set; }
    }
}
