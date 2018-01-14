using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.Model
{
    public class Nachricht
    {
        public bool success { get; set; }
        public string data { get; set; }
        public string responseType { get; set; }
        public object message { get; set; }
        public object title { get; set; }
    }
}
