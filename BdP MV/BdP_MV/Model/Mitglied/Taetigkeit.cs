using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace BdP_MV.Model.Mitglied
{

    public class Taetigkeit
    {
        public DateTime? entries_aktivBis { get; set; }
        public string entries_beitragsArt { get; set; }
        public string entries_caeaGroup { get; set; }
        public DateTime? entries_aktivVon { get; set; }
        public string descriptor { get; set; }
        public string representedClass { get; set; }
        public DateTime? entries_anlagedatum { get; set; }
        public string entries_caeaGroupForGf { get; set; }
        public string entries_untergliederung { get; set; }
        public string entries_taetigkeit { get; set; }
        public string entries_gruppierung { get; set; }
        public int id { get; set; }
        public string entries_mitglied { get; set; }
        [JsonIgnore]
        public Boolean aktiv { get; set; }
    }

    public class Field
    {
        public string name { get; set; }
        public object header { get; set; }
        public bool hidden { get; set; }
        public int width { get; set; }
        public int? flex { get; set; }
    }

    public class MetaData
    {
        public string totalProperty { get; set; }
        public string root { get; set; }
        public string id { get; set; }
        public List<Field> fields { get; set; }
    }

    public class RootObject_Taetigkeit
    {
        public bool success { get; set; }
        public List<Taetigkeit> data { get; set; }
        public string responseType { get; set; }
        public int totalEntries { get; set; }
        public MetaData metaData { get; set; }
    }
}
