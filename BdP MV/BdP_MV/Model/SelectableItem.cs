using System;
using System.Collections.Generic;

namespace BdP_MV
{
    public class SelectableItem
    {
        public string Id { get; set; }
        public string name { get; set; }
        public string descriptor { get; set; }
        public string representedClass { get; set; }
    }
    public class RootObjectItem
    {
        public bool success { get; set; }
        public List<SelectableItem> data { get; set; }
        public string responseType { get; set; }
        public int totalEntries { get; set; }
    }
}
