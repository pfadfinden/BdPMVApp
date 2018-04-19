using System;
using System.Collections.Generic;
using System.Text;

namespace BdP_MV.Model.Mitglied
{
    public class SGB8
    {
        public DateTime? entries_erstelltAm { get; set; }
        public string entries_fzNummer { get; set; }
        public string entries_empfaenger { get; set; }
        public string entries_empfNachname { get; set; }
        public string entries_empfVorname { get; set; }
        public DateTime? entries_empfGebDatum { get; set; }
        public int id { get; set; }
        public string descriptor { get; set; }
        public string entries_datumEinsicht { get; set; }
        public string representedClass { get; set; }
        public DateTime? entries_fzDatum { get; set; }
        public string entries_autor { get; set; }
    }

  
    public class RootObject_SGB8
    {
        public bool success { get; set; }
        public List<SGB8> data { get; set; }
        public string responseType { get; set; }
        public int totalEntries { get; set; }
        public MetaData metaData { get; set; }
    }
}
