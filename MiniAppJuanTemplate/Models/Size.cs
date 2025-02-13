using MiniAppJuanTemplate.Models.Common;

namespace MiniAppJuanTemplate.Models
{
	public class Size:BaseEntity
	{
		public string Value {  get; set; }
	    public List<ProductSize>? ProductSizes { get; set; }

	}
}
