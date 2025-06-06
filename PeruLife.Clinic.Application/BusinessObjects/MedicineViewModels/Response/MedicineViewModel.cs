﻿namespace PureLifeClinic.Application.BusinessObjects.MedicineViewModels.Response
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
}
