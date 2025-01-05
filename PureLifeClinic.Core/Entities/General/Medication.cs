using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class Medication : Base<int>
    {

        [Required, StringLength(100)]
        public string Name { get; set; } 

        [StringLength(500)]
        public string Description { get; set; } 

        public double Price { get; set; } 

        public int StockQuantity { get; set; } 

        [StringLength(100)]
        public string Manufacturer { get; set; } 
    }
}
