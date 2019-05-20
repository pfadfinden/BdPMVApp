using BdP_MV.Exceptions;
using BdP_MV.Model;
using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.ViewModel
{
    public class NewMitgliedViewModel : BaseNavigationViewModel
    {
        public MainController mainC;
        public List<SelectableItem> geschlechter;
        public List<SelectableItem> beitragsart;
        public List<SelectableItem> mitgltype;
        public List<SelectableItem> land;
        public MitgliedDetails mitglied;
        private int gruppierungsID;

        public NewMitgliedViewModel(int gruppenID)
        {
            mainC = new MainController();
            gruppierungsID = gruppenID;
            mitglied = new MitgliedDetails();


        }
        public async Task GenerateJSON(int idGruppe)
        {
           
           
            if (String.IsNullOrEmpty(mitglied.vorname) || String.IsNullOrEmpty(mitglied.nachname) || string.IsNullOrEmpty(mitglied.beitragsart) || string.IsNullOrEmpty(mitglied.geschlecht) || string.IsNullOrEmpty(mitglied.strasse))
            {
                throw new NotAllRequestedFieldsFilledException("Die erforderlichen Felder wurden nicht ausgewählt.");
            }
            if ((String.IsNullOrEmpty(mitglied.strasse) || String.IsNullOrEmpty(mitglied.plz) || String.IsNullOrEmpty(mitglied.ort)) && mitglied.zeitschriftenversand)
                {
                throw new NotAllRequestedFieldsFilledException("Ein Zeitschriftenversand ist ohne komplette Adressangabe nicht möglich.");

            }
            mitglied.gruppierungId = idGruppe;



            string JSONOutput = JsonConvert.SerializeObject(mitglied,
                           Newtonsoft.Json.Formatting.None,
                           new JsonSerializerSettings
                           {
                               NullValueHandling = NullValueHandling.Ignore, DefaultValueHandling = DefaultValueHandling.Ignore, Formatting = Formatting.Indented
                           });
            Console.WriteLine(JSONOutput);

            await mainC.mVConnector.PostNewMitglied(idGruppe, JSONOutput);
        }
        public NewMitgliedViewModel(MitgliedDetails uebergebenesMitglied)
        {
            mainC = new MainController();
            mitglied = uebergebenesMitglied;
            gruppierungsID = uebergebenesMitglied.gruppierungId;
            


        }
        public async Task loadSelectableItems()
        {
            Task <List<SelectableItem>> taskGeschlechter = mainC.mVConnector.GetItems("baseadmin/geschlecht/");
            Task<List<SelectableItem>> taskBeitragsart = mainC.mVConnector.GetItems("namiBeitrag/beitragsartmgl/gruppierung/"+gruppierungsID+"/");
            Task<List<SelectableItem>> taskMitgliedstyp = mainC.mVConnector.GetItems("nami/taetigkeitaufgruppierung/filtered/gruppierung/erste-taetigkeit/gruppierung/"+gruppierungsID);
            Task<List<SelectableItem>> taskLand = mainC.mVConnector.GetItems("baseadmin/land/");
            await Task.WhenAll(taskGeschlechter, taskBeitragsart, taskMitgliedstyp, taskLand);
             geschlechter = await taskGeschlechter;
            beitragsart = await taskBeitragsart;
            mitgltype = await taskMitgliedstyp;
            land = await taskLand;
         }


    }
}
