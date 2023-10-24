using Microsoft.AspNetCore.Identity;

namespace Gateway.Models
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<Text>? Texts { get; set; }
    }
}
