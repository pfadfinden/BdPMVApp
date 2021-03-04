using BdP_MV.Model.Mitglied;

namespace BdP_MV.Model.Metamodel
{
    class RootObj_edit_Mitglied
    {
        public bool success { get; set; }
        public MitgliedDetails data { get; set; }
        public string responseType { get; set; }
        public string message { get; set; }
        public object title { get; set; }
    }
}
