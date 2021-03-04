using System.Collections.Generic;

namespace BdP_MV.Model
{
    public class Gruppe
    {
        public string descriptor { get; set; }
        public string name { get; set; }
        public string representedClass { get; set; }
        public int id { get; set; }
    }

    public class GroupList
    {
        public bool success { get; set; }
        public List<Gruppe> data { get; set; }
        public string responseType { get; set; }
        public string message { get; set; }
    }
}
