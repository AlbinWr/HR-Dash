using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models
{
    public class Mote
    {
        public int MoteId { get; set; }

        [Required(ErrorMessage = "Titel krävs")]
        public string Titel { get; set; }
        public DateTime StartTid { get; set; }
        public DateTime SlutTid { get; set; }
        public string? Beskrivning { get; set; }

        [Required(ErrorMessage = "Plats krävs")]
        public string? Plats { get; set; }

        // Koppling till anställda
        public List<MoteAnstalld> MoteAnstallda { get; set; }

        // Koppling till manager
        public string ManagerId { get; set; }
        public ApplicationUser Manager { get; set; }
    }
}
