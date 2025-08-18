using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models.ViewModels
{
    public class RegistreraViewModel
    {
        public int AnstalldId { get; set; }

        [Required(ErrorMessage = "Namn krävs")]
        public string Namn { get; set; }

        [Required(ErrorMessage = "E-postadress krävs")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Lösenord krävs")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Lösenordet måste vara minst {2} tecken långt.")]
        [DataType(DataType.Password)]
        [Compare("BekraftaLosenord", ErrorMessage = "Lösenorden matchar inte.")]
        public string Losenord { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenord")]
        public string BekraftaLosenord { get; set; }

        [Required]
        [Display(Name = "Anställningsdatum")]
        [DataType(DataType.Date)]
        public DateTime AnstallningDatum { get; set; } = DateTime.Today;

        public bool Manager { get; set; }

        /*
        [Required(ErrorMessage = "Telefonnummer krävs")]
        public string Telefonnummer { get; set; }
        */
    }
}
