using System.Collections.Generic;

namespace BdP_MV.Model
{

    public class TabGroupAssociation
    {
        public int tabGroupId { get; set; }
        public int fieldPosition { get; set; }
    }

    public class Meta_Field
    {
        public string name { get; set; }
        public string type { get; set; }
        public string inputType { get; set; }
        public bool visible { get; set; }
        public string enabled { get; set; }
        public string serviceUrl { get; set; }
        public bool searchable { get; set; }
        public bool sortable { get; set; }
        public bool listable { get; set; }
        public string labelId { get; set; }
        public string label { get; set; }
        public int order { get; set; }
        public List<TabGroupAssociation> tabGroupAssociation { get; set; }
        public bool hidden { get; set; }
        public string columnCssClass { get; set; }
        public string tooltipText { get; set; }
        public string labelCssClass { get; set; }
        public bool mandatory { get; set; }
        public List<string> actions { get; set; }
        public bool? allowDecimals { get; set; }
        public object minValue { get; set; }
        public object maxValue { get; set; }
        public string regex { get; set; }
        public string regexText { get; set; }
        public List<object> dependentOn { get; set; }
        public int? pageSize { get; set; }
        public bool? specialRowSelection { get; set; }
        public string queryMode { get; set; }
        public string autoLoad { get; set; }
        public bool? multiSelect { get; set; }
        public bool? withTime { get; set; }
        public bool? withSeconds { get; set; }
    }

    public class ExtraAction
    {
        public string url { get; set; }
        public string type { get; set; }
        public string label { get; set; }
        public int width { get; set; }
        public int height { get; set; }
        public string iconCssClass { get; set; }
        public string httpMethod { get; set; }
        public string jsClass { get; set; }
    }

    public class TabGroup
    {
        public int id { get; set; }
        public string label { get; set; }
        public int position { get; set; }
    }

    public class AssociatedFilterableEntity
    {
        public string name { get; set; }
        public string label { get; set; }
        public string baseClass { get; set; }
        public string relClass { get; set; }
        public string serviceUrl { get; set; }
        public string type { get; set; }
        public object readOnly { get; set; }
        public string filteringPropertyName { get; set; }
    }

    public class Meta_Data
    {
        public List<Meta_Field> fields { get; set; }
        public List<object> relations { get; set; }
        public List<string> actions { get; set; }
        public string packageName { get; set; }
        public string simpleClassName { get; set; }
        public bool flist { get; set; }
        public int pageSize { get; set; }
        public string label { get; set; }
        public bool description { get; set; }
        public List<ExtraAction> extraActions { get; set; }
        public bool groupable { get; set; }
        public List<TabGroup> tabGroups { get; set; }
        public string groupingType { get; set; }
        public bool loadDefaultEntity { get; set; }
        public string mainDataTabLabel { get; set; }
        public bool withActionsOnMultipleRows { get; set; }
        public bool tpWithActions { get; set; }
        public string uniqueId { get; set; }
        public object tpFilteringPropertyName { get; set; }
        public bool multiColumnSort { get; set; }
        public List<AssociatedFilterableEntity> associatedFilterableEntities { get; set; }
        public bool extraActionsInExtraToolbar { get; set; }
    }

    public class RootObject_Meta_Data
    {
        public bool success { get; set; }
        public Meta_Data data { get; set; }
        public string responseType { get; set; }
        public object message { get; set; }
        public object title { get; set; }
    }
}
