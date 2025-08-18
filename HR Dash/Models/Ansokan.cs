using System.ComponentModel.DataAnnotations;

namespace HR_Dash.Models
{
    public class Ansokan
    {
        [Key]
        public int AnsokanId { get; set; }
        public DateTime StartDatum { get; set; }
        public DateTime SlutDatum { get; set; }
        public string AnsokanTyp { get; set; } = string.Empty;

        public int AnstalldId { get; set; }
        public Anstalld Anstalld { get; set; } = null!; //Navigation property
    }
}
