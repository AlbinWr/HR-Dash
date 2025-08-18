using HR_Dash.Models;
using System.ComponentModel.DataAnnotations;

public class AnstalldSchema
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int AnstalldId { get; set; }
    public Anstalld Anstalld { get; set; } = null!;

    [Required]
    public int RullandeSchemaId { get; set; }
    public RullandeSchema RullandeSchema { get; set; } = null!;

    [Required]
    public DateTime Startdatum { get; set; }
}