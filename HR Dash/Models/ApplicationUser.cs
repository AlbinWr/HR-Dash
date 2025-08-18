using Microsoft.AspNetCore.Identity;
namespace HR_Dash.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Anstalld? Anstalld { get; set; } // Navigation property
    }
}
