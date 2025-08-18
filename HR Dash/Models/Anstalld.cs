using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models
{
    public class Anstalld
    {
        [Key]
        public int AnstalldId { get; set; }
        public string AnstalldName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefonnummer { get; set; } = string.Empty;
        public DateTime AnstallningDatum { get; set; }
        public string Avdelning { get; set; } = string.Empty;
        public string ProfilBild { get; set; } = string.Empty; //URL

        public ICollection<Ansokan> Ansokningar { get; set; } = new List<Ansokan>(); //Navigation property
        public ICollection<Skift> Skift { get; set; } = new List<Skift>(); //Navigation property

        public string ApplicationUserId { get; set; } = string.Empty;
        public ApplicationUser ApplicationUser { get; set; } = null!;
    }
}
