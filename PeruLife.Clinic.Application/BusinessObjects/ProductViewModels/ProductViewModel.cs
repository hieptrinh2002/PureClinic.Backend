namespace PureLifeClinic.Application.BusinessObjects.ProductViewModels
{
    public class ProductViewModel
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
