using System;

namespace BdP_MV.Model.Mitglied
{
    public class Ausbildung_Details
    {
        public int id { get; set; }
        public string baustein { get; set; }
        public int bausteinId { get; set; }
        public string mitglied { get; set; }
        public DateTime? vstgTag { get; set; }
        public string vstgName { get; set; }
        public string veranstalter { get; set; }
        public object lastModifiedFrom { get; set; }
    }

    public class RootObject_Ausbildung_Details
    {
        public bool success { get; set; }
        public Ausbildung_Details data { get; set; }
        public object responseType { get; set; }
        public object message { get; set; }
        public object title { get; set; }
    }
}
