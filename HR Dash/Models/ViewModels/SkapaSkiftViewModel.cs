using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models.ViewModels
{
    public class SkapaSkiftViewModel
    {
        [Required(ErrorMessage = "Välj en anställd")]
        public int AnstalldId { get; set; }

        [Required(ErrorMessage = "Starttid krävs")]
        public DateTime StartTid { get; set; }

        [Required(ErrorMessage = "Sluttid krävs")]
        public DateTime SlutTid { get; set; }

        public List<SelectListItem> AnstalldaList { get; set; } = new();
    }
}
