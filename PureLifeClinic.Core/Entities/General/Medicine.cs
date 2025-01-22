﻿using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class Medicine : Base<int>
    {
        [Required, StringLength(maximumLength: 8, MinimumLength = 2)]
        public string Code { get; set; }
        [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        public int Quantity { get; set; }
        [StringLength(maximumLength: 350)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }

        [StringLength(100)]
        public string? Manufacturer { get; set; } 
    }
}
