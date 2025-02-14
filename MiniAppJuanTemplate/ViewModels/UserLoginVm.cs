using System.ComponentModel.DataAnnotations;

namespace MiniAppJuanTemplate.ViewModels
{
    public class UserLoginVm
    {
        public string UserNameOrEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}
