using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

public class TilldelaSchemaViewModel
{
    public int IdRullandeSchema { get; set; }

    public string NamnSkift { get; set; } = string.Empty;

    [Required(ErrorMessage = "Minst en anställd måste väljas")]
    public List<int> ValdaAnstallda { get; set; } = new();

    public List<SelectListItem> AnstalldaLista { get; set; } = new();

    [Required(ErrorMessage = "Startdatum krävs")]
    public DateTime Startdatum { get; set; } = DateTime.Today;
}