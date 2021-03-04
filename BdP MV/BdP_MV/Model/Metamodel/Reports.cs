using System.Collections.Generic;

namespace BdP_MV.Model.Metamodel

/* Nicht gemergte Änderung aus Projekt "BdP_MV.iOS"
Vor:
{
    
    
    public class Report_Data
Nach:
{


    public class Report_Data
*/
{


    public class Report_Data
    {
        public int entries_displayOrder { get; set; }
        public string entries_name { get; set; }
        public string entries_reportXmlFile { get; set; }
        public string entries_menuItem { get; set; }
        public string entries_reportFileName { get; set; }
        public object entries_subreportId { get; set; }
        public string entries_info { get; set; }
        public string entries_defaultReportClazz { get; set; }
        public string descriptor { get; set; }
        public int entries_id { get; set; }
        public string representedClass { get; set; }
        public string entries_gruppierung { get; set; }
        public string entries_ebene { get; set; }
        public int id { get; set; }
        public bool entries_mitEbeneHierarchie { get; set; }
        public string entries_reportResultType { get; set; }
    }

    public class Report_Field
    {
        public string name { get; set; }
        public object header { get; set; }
        public bool hidden { get; set; }
        public int width { get; set; }
        public int? flex { get; set; }
    }

    public class Report_MetaData
    {
        public string totalProperty { get; set; }
        public string root { get; set; }
        public string id { get; set; }
        public List<Report_Field> fields { get; set; }
    }

    public class Report_RootObject
    {
        public bool success { get; set; }
        public List<Report_Data> data { get; set; }
        public string responseType { get; set; }
        public int totalEntries { get; set; }
        public Report_MetaData metaData { get; set; }
        public string message { get; set; }

    }


}
