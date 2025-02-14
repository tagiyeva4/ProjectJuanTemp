using Microsoft.AspNetCore.Identity;

namespace MiniAppJuanTemplate.Models
{
    public class AppUser:IdentityUser
    {
        public string? FullName { get; set; }
    }
}
