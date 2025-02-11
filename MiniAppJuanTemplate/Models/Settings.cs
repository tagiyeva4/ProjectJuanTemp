using System.ComponentModel.DataAnnotations;

namespace MiniAppJuanTemplate.Models
{
    public class Settings
    {
        [Key]
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
