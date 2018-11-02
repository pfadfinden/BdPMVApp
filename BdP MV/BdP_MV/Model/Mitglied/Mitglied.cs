using System;
using System.Collections.Generic;


namespace BdP_MV.Model.Mitglied
{
    public class Mitglied
    {
        public object entries_ersteTaetigkeitId { get; set; }
        public string entries_genericField1 { get; set; }
        public int entries_version { get; set; }
        public string entries_telefon3 { get; set; }
        public string entries_telefon2 { get; set; }
        public string entries_telefon1 { get; set; }
        public string descriptor { get; set; }
        public int entries_id { get; set; }
        public string entries_staatsangehoerigkeit { get; set; }
        public string representedClass { get; set; }
        public string entries_rover { get; set; }
        public string entries_pfadfinder { get; set; }
        public int entries_mitgliedsNummer { get; set; }
        public bool entries_wiederverwendenFlag { get; set; }
        public object entries_ersteUntergliederungId { get; set; }
        public string entries_rowCssClass { get; set; }
        public string entries_vorname { get; set; }
        public int id { get; set; }
        public string entries_woelfling { get; set; }
        public int entries_gruppierungId { get; set; }

        public string entries_beitragsarten { get; set; }
        public string entries_stufe { get; set; }
        public string entries_email { get; set; }
        public string entries_konfession { get; set; }
        public string entries_emailVertretungsberechtigter { get; set; }
        public string entries_fixBeitrag { get; set; }
        public string entries_lastUpdated { get; set; }
        public string entries_status { get; set; }
        public string entries_jungpfadfinder { get; set; }
        public string entries_mglType { get; set; }
        public string entries_kontoverbindung { get; set; }
        public string entries_geschlecht { get; set; }
        public string entries_spitzname { get; set; }
        public DateTime? entries_geburtsDatum { get; set; }
        public string entries_staatangehoerigkeitText { get; set; }
        public string entries_nachname { get; set; }
        public DateTime? entries_eintrittsdatum { get; set; }
        public DateTime? entries_austrittsDatum { get; set; }
        public string entries_genericField2 { get; set; }
        public string entries_telefax { get; set; }
        public string ansprechname { get; set; }
        
        public string Gruppe { get; set; }
    }

    public class MitgliederField
    {
        public string name { get; set; }
        public object header { get; set; }
        public bool hidden { get; set; }
        public int width { get; set; }
        public int? flex { get; set; }
    }

    public class MitgliederMetaData
    {
        public string totalProperty { get; set; }
        public string root { get; set; }
        public string id { get; set; }
        public List<MitgliederField> fields { get; set; }
    }

    public class MitgliederListe
    {
        public bool success { get; set; }
        public List<Mitglied> data { get; set; }
        public string responseType { get; set; }
        public int totalEntries { get; set; }
        public MitgliederMetaData metaData { get; set; }
    }
}
