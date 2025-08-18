using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models
{
    public class Skift
    {
        [Key]
        public int IdSkift { get; set; }
        [Required]
        public DateTime StartTid { get; set; }
        [Required]
        public DateTime SlutTid { get; set; }

        [Required]
        public int AnstalldId { get; set; }
        public Anstalld Anstalld { get; set; } = null!; //Navigation property

        public int? AnstalldSchemaId { get; set; }
        public AnstalldSchema? AnstalldSchema { get; set; } = null!;

        public int? RullandeSchemaId { get; set; }
        public RullandeSchema RullandeSchema { get; set; }
    }
}
