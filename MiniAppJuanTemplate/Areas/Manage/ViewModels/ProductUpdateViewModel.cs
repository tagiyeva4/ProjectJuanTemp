namespace MiniAppJuanTemplate.Areas.Manage.ViewModels
{
    public class ProductUpdateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal DiscountPercentege { get; set; }
        public bool IsStock { get; set; }
        public bool IsNew { get; set; }
        public int Rate { get; set; }
        public int CategoryId { get; set; }
        public List<int> TagIds { get; set; }
        public List<int> SizeIds { get; set; }
        public IFormFile[] Photos { get; set; }
        public IFormFile MainPhoto { get; set; }
    }
}
