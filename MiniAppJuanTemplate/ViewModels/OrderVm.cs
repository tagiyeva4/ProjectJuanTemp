using System.ComponentModel.DataAnnotations;

namespace MiniAppJuanTemplate.ViewModels
{
    public class OrderVm
    {
        public string? Country { get; set; }
        public string? CompanyName { get; set; }
        public string? TownCity { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string? PhoneNumber { get; set; }
        public string? State { get; set; }
        public string? StreetAddress { get; set; }
        public string? ZipCode { get; set; }
    }
}
