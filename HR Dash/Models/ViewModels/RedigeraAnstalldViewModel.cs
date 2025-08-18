using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models.ViewModels
{
    public class RedigeraAnstalldViewModel
    {
        public int AnstalldId { get; set; }

        //Identity
        [Required(ErrorMessage = "E-post krävs")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
        [Display(Name = "E-post")]
        public string Email { get; set; } = string.Empty;

        //Anställd
        [Required(ErrorMessage = "Anställd namn krävs")]
        [Display(Name = "Namn")]
        public string Namn { get; set; } = string.Empty;

        [Display(Name = "Telefonnummer")]
        public string? Telefonnummer { get; set; }

        [Required]
        [Display(Name = "Anställningsdatum")]
        [DataType(DataType.Date)]
        public DateTime AnstallningDatum { get; set; } = DateTime.Today;

        public bool Manager { get; set; }
    }
}
