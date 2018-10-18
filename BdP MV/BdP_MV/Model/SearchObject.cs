using System;
using System.Collections.Generic;
using System.Text;

namespace BdP_MV.Model
{
    public class SearchObject
    {
        public string vorname { get; set; }
        public string nachname { get; set; }
        public string spitzname { get; set; }
        public string mitgliedsNummber { get; set; }
        public string mglWohnort { get; set; }
        public string alterVon { get; set; }
        public string alterBis { get; set; }
        public string mglStatusId { get; set; }
        public string funktion { get; set; }
        public string organisation { get; set; }

        public bool zeitschriftenversand { get; set; }
        public string searchName { get; set; }
        public bool mitAllenTaetigkeiten { get; set; }
        public bool withEndedTaetigkeiten { get; set; }
        //public object ebeneId { get; set; }
        public string grpNummer { get; set; }
        public string grpName { get; set; }
        //public object gruppierung1Id { get; set; }
        //public object gruppierung2Id { get; set; }
        //public object gruppierung3Id { get; set; }
        //public object gruppierung4Id { get; set; }
        //public object gruppierung5Id { get; set; }
        //public object gruppierung6Id { get; set; }
        public bool inGrp { get; set; }
        public bool unterhalbGrp { get; set; }
        public string privacy { get; set; }
        public string searchType { get; set; }
    }
}
