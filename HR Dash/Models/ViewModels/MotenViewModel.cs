namespace HR_Dash.Models.ViewModels
{
    public class MotenViewModel
    {
        public int MoteId { get; set; }
        public string Titel { get; set; }
        public string Plats { get; set; }
        public DateTime StartTid { get; set; }
        public DateTime SlutTid { get; set; }
        public List<string> Deltagare { get; set; }
        public bool IsManager { get; set; }
    }
}
