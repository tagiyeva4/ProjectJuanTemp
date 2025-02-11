using MiniAppJuanTemplate.Models.Common;

namespace MiniAppJuanTemplate.Models
{
    public class Blog:BaseEntity
    {
        public string Image {  get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string WriteBy { get; set; }
        public DateTime WriteOn { get; set; }
        public string ButtonText { get; set; }
    }
}
