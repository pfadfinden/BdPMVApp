using Newtonsoft.Json;
using System;

namespace BdP_MV.Model.Mitglied
{
    public class KontoverbindungMitglied
    {
        public int? id { get; set; }
        public string institut { get; set; }
        public string bankleitzahl { get; set; }
        public string kontonummer { get; set; }
        public string iban { get; set; }
        public string bic { get; set; }
        public string kontoinhaber { get; set; }
        public int? mitgliedsNummer { get; set; }
        public object zahlungsKonditionId { get; set; }
        public object zahlungsKondition { get; set; }
    }

    public class MitgliedDetails
    {
        public string dyn_eMail2 { get; set; }
        public object jungpfadfinder { get; set; }
        public string mglType { get; set; }
        public string geschlecht { get; set; }
        public object staatsangehoerigkeit { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object ersteTaetigkeitId { get; set; }
        public object ersteUntergliederung { get; set; }
        public string lastUpdated { get; set; }
        public string emailVertretungsberechtigter { get; set; }
        public object ersteTaetigkeit { get; set; }
        public string nameZusatz { get; set; }
        public int id { get; set; }
        public string dyn_BegruendungStamm { get; set; }
        public object staatsangehoerigkeitId { get; set; }
        public int version { get; set; }
        public bool sonst01 { get; set; }
        public bool sonst02 { get; set; }
        public string spitzname { get; set; }
        public int? landId { get; set; }
        public string staatsangehoerigkeitText { get; set; }
        public int gruppierungId { get; set; }
        public string mglTypeId { get; set; }
        public string beitragsart { get; set; }
        public string nachname { get; set; }
        public DateTime? eintrittsdatum { get; set; }
        public object rover { get; set; }
        public object region { get; set; }
        public string status { get; set; }
        public object konfession { get; set; }
        public object fixBeitrag { get; set; }
        public object konfessionId { get; set; }
        public bool zeitschriftenversand { get; set; }
        public object pfadfinder { get; set; }
        public string telefon3 { get; set; }
        public KontoverbindungMitglied kontoverbindung { get; set; }
        public int? geschlechtId { get; set; }
        public string land { get; set; }
        public string email { get; set; }
        public string telefon1 { get; set; }
        public object woelfling { get; set; }
        public string telefon2 { get; set; }
        public string strasse { get; set; }
        public string vorname { get; set; }
        public string dyn_BegruendungMitglied { get; set; }
        public int mitgliedsNummer { get; set; }
        public string gruppierung { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime? austrittsDatum { get; set; }
        public string ort { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public object ersteUntergliederungId { get; set; }

        public bool wiederverwendenFlag { get; set; }
        public string dyn_Bemerkung { get; set; }
        public object regionId { get; set; }
        public DateTime? geburtsDatum { get; set; }
        public object stufe { get; set; }
        public string genericField1 { get; set; }
        public string genericField2 { get; set; }
        public string telefax { get; set; }
        public int? beitragsartId { get; set; }
        public string plz { get; set; }
        [JsonIgnore]
        public string gruppe { get; set; }
        [JsonIgnore]
        public int alter { get; set; }
        [JsonIgnore]
        public string ansprechname { get; set; }
        [JsonIgnore]
        public string kleingruppe { get; set; }
    }

    public class RootObjectMitgliedDetails
    {
        public bool success { get; set; }
        public MitgliedDetails data { get; set; }
        public object responseType { get; set; }
        public object message { get; set; }
        public object title { get; set; }
    }
}
