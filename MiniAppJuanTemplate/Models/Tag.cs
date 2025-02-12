using MiniAppJuanTemplate.Models.Common;

namespace MiniAppJuanTemplate.Models
{
    public class Tag:BaseAuditableEntity
    {
       public string Name { get; set; }
        public List<Product> Products { get; set; }

    }
}
