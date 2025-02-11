using MiniAppJuanTemplate.Models.Common;

namespace MiniAppJuanTemplate.Models
{
    public class Category:BaseEntity
    {
        public string Name {  get; set; }
        public List<Product> Products { get; set; }
    }
}
