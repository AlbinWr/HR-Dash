using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "E-postadress krävs")]
        [EmailAddress(ErrorMessage = "Ogiltig e-postadress")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Lösenord krävs")]
        [DataType(DataType.Password)]
        public string Losenord { get; set; }
        [Display(Name = "Kom ihåg mig?")]
        public bool KomIhag { get; set; }
    }
}
