using System.ComponentModel.DataAnnotations;

namespace PureLifeClinic.Core.Entities.General
{
    public class Medication : Base<int>
    {

        [Required, StringLength(100)]
        public string Name { get; set; } // Tên thuốc

        [StringLength(500)]
        public string Description { get; set; } // Mô tả thuốc

        public decimal Price { get; set; } // Giá thuốc

        public int StockQuantity { get; set; } // Số lượng thuốc còn trong kho

        [StringLength(100)]
        public string Manufacturer { get; set; } // Nhà sản xuất
    }
}
