using System;
using System.Collections.Generic;

namespace BdP_MV
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string representedClass { get; set; }
    }
    public class RootObjectItem
    {
        public bool success { get; set; }
        public List<Item> data { get; set; }
        public string responseType { get; set; }
        public int totalEntries { get; set; }
    }
}
