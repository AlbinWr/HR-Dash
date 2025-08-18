using System;
using System.Collections.Generic;

namespace HR_Dash.Models.ViewModels
{
    public class StartsidaViewModel
    {
        public Skift? NastaSkift { get; set; }
        public List<MoteInfo> KommandeMoten { get; set; } = new();

        public class MoteInfo
        {
            public string Titel { get; set; } = "";
            public DateTime StartTid { get; set; }
            public string Plats { get; set; } = "";
        }
    }
}