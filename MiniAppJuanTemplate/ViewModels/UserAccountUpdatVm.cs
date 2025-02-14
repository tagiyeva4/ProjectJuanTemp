using System.ComponentModel.DataAnnotations;

namespace MiniAppJuanTemplate.ViewModels
{
    public class UserAccountUpdatVm
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        [DataType(DataType.Password)]
        public string? CurrentPasword { get; set; }
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [DataType(DataType.Password)]
        [Compare(nameof(NewPassword))]
        public string? ConfirmPassword { get; set; }
    }
}
