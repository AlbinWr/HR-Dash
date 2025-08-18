using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models.ViewModels
{
    public class AndraLosenordViewModel
    {
        public int AnstalldId { get; set; }

        [Required(ErrorMessage = "Gammalt lösenord krävs")]
        [DataType(DataType.Password)]
        [Display(Name = "Nuvarande lösenord")]
        public string NuvarandeLosenord { get; set; }

        [Required(ErrorMessage = "Lösenord krävs")]
        [StringLength(40, MinimumLength = 8, ErrorMessage = "Lösenordet måste vara minst {2} tecken långt.")]
        [DataType(DataType.Password)]
        [Compare("BekraftaNyttLosenord", ErrorMessage = "Lösenorden matchar inte.")]
        public string NyttLosenord { get; set; }

        [Required(ErrorMessage = "Användarnamn krävs")]
        [DataType(DataType.Password)]
        [Display(Name = "Bekräfta lösenord")]
        public string BekraftaNyttLosenord { get; set; }
    }
}
