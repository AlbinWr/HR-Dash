using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models
{
    public class RullandeSchema
    {
        [Key]
        public int IdSchema { get; set; }

        [Required]
        public string NamnSkift { get; set; } = string.Empty;

        [Required(ErrorMessage = "StartTid krävs")]
        public TimeSpan StartTid { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "Timmar måste vara mellan 1 och 12")]
        public int SkiftTimmar { get; set; }

        [Required]
        [Range(0, 59, ErrorMessage = "Minuter måste vara mellan 0 och 59")]
        public int SkiftMinuter { get; set; }

        [Required(ErrorMessage = "Mönster krävs")]
        public string MonsterString { get; set; } = null!; // Ex: "J4L4J5L5"

        //public DateTime SkapaSkiftTillDatum { get; set; }  // Ex: Skapa skift till 2025-12-31

        public ICollection<AnstalldSchema> Anstallda { get; set; } = new List<AnstalldSchema>();
    }
}
