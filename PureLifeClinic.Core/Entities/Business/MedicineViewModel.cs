﻿using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.Business
{
    public class MedicineViewModel
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class MedicineCreateViewModel
    {
        [Required, StringLength(maximumLength: 8, MinimumLength = 2)]
        public string? Code { get; set; }
        [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
        public string? Name { get; set; }
        [Required, Range(0.01, float.MaxValue)]
        public double Price { get; set; }
        public int Quantity { get; set; }
        [StringLength(maximumLength: 350)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class MedicineUpdateViewModel
    {
        public int Id { get; set; }
        [Required, StringLength(maximumLength: 8, MinimumLength = 2)]
        public string? Code { get; set; }
        [Required, StringLength(maximumLength: 100, MinimumLength = 2)]
        public string? Name { get; set; }
        [Required, Range(0.01, float.MaxValue)]
        public double Price { get; set; }
        public int Quantity { get; set; }
        [StringLength(maximumLength: 350)]
        public string? Description { get; set; }
        public bool IsActive { get; set; }
    }
}
