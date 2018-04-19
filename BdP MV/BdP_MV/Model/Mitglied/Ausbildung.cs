using System;
using System.Collections.Generic;
using System.Text;

namespace BdP_MV.Model.Mitglied
{
    public class Ausbildung
    {
        public string entries_vstgTag { get; set; }
        public string entries_veranstalter { get; set; }
        public string entries_vstgName { get; set; }
        public string entries_baustein { get; set; }
        public int id { get; set; }
        public string descriptor { get; set; }
        public int entries_id { get; set; }
        public string representedClass { get; set; }
        public string entries_mitglied { get; set; }
    }


   

    public class RootObject_Ausbildung
    {
        public bool success { get; set; }
        public List<Ausbildung> data { get; set; }
        public string responseType { get; set; }
        public int totalEntries { get; set; }
        public MetaData metaData { get; set; }
        public string message { get; set; }
    }
}
