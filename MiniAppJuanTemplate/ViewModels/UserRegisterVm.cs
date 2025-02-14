using System.ComponentModel.DataAnnotations;

namespace MiniAppJuanTemplate.ViewModels
{
    public class UserRegisterVm
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare(nameof(Password))]
		[DataType(DataType.Password)]
		public string ConfirmPassword { get; set; }
        public bool Subscribe {  get; set; }
    }
}
