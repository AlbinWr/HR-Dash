using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models.ViewModels
{
    public class RedigeraProfilViewModel
    {
        public int AnstalldId { get; set; }

        [Required]
        [Display(Name = "Namn")]
        public string AnstalldName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [Display(Name = "E-post")]
        public string Email { get; set; } = string.Empty;

        [Phone]
        [Display(Name = "Telefonnummer")]
        public string? Telefonnummer { get; set; }
    }
}
