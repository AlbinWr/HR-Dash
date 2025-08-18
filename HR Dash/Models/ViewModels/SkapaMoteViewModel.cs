using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models.ViewModels
{
    public class SkapaMoteViewModel
    {
        public int? MoteId { get; set; }

        [Required(ErrorMessage = "Titel krävs")]
        public string Titel { get; set; }

        [Required(ErrorMessage = "Plats krävs")]
        public string Plats { get; set; }

        public string? Beskrivning { get; set; }

        [Required(ErrorMessage = "Datum krävs")]
        [DataType(DataType.Date)]
        public DateTime Datum { get; set; }

        [Required(ErrorMessage = "Tid krävs")]
        [DataType(DataType.Time)]
        public TimeSpan Tid { get; set; }

        [ValidateNever]
        public List<SelectListItem> StartTidAlternativ { get; set; } 

        [ValidateNever]
        public List<SelectListItem> LangdAlternativ { get; set; } 

        [Required(ErrorMessage = "Längd på möte krävs")]
        [Range(15, 60, ErrorMessage = "Längden måste vara mellan 15 och 60 minuter")]
        public int MoteLangd { get; set; }

        [Required(ErrorMessage = "Minst en anställd måste väljas")]
        public List<int> ValdaAnstallda { get; set; }

        [ValidateNever]
        public List<SelectListItem> AnstalldaList { get; set; }
    }
}
