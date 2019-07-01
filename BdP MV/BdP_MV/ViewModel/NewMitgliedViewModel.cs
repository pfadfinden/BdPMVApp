using BdP_MV.Exceptions;
using BdP_MV.Ext_Packages;
using BdP_MV.Model;
using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
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
        public NewMitgliedViewModel(MitgliedDetails mitgliedDetail)
        {
            mainC = new MainController();
            gruppierungsID = mitglied.gruppierungId;
            mitglied = mitgliedDetail;

        }
        public async Task<String> GenerateJSON(int idGruppe)
        {
            IsBusy = true;
            

            if (String.IsNullOrEmpty(mitglied.vorname) || String.IsNullOrEmpty(mitglied.nachname) || mitglied.beitragsartId==0 || mitglied.geschlechtId == 0 || string.IsNullOrEmpty(mitglied.strasse)|| !mitglied.eintrittsdatum.HasValue||!mitglied.geburtsDatum.HasValue)
            {
                throw new NotAllRequestedFieldsFilledException("Die erforderlichen Felder wurden nicht ausgewählt.");
            }
            if ((String.IsNullOrEmpty(mitglied.strasse) || String.IsNullOrEmpty(mitglied.plz) || String.IsNullOrEmpty(mitglied.ort)) && mitglied.zeitschriftenversand)
                {
                throw new NotAllRequestedFieldsFilledException("Ein Zeitschriftenversand ist ohne komplette Adressangabe nicht möglich.");

            }
            int age = mainC.mitgliederController.GetAgeFromDate((DateTime)mitglied.geburtsDatum);
            if (age>17&&(String.IsNullOrEmpty(mitglied.dyn_BegruendungMitglied)|| String.IsNullOrEmpty(mitglied.dyn_BegruendungStamm)))
            {
                throw new NotAllRequestedFieldsFilledException("Du darfst keine Ü18-Mitglieder ohne Begründung anlegen.");

            }
            mitglied.wiederverwendenFlag = true;
            
            mitglied.gruppierungId = idGruppe;
            mitglied.kontoverbindung = new KontoverbindungMitglied();



            string JSONOutput = JsonConvert.SerializeObject(mitglied,
                           Newtonsoft.Json.Formatting.None,
                           new JsonSerializerSettings
                           {
                               DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                               ContractResolver = new NullToEmptyStringResolver()

                           });
            Console.WriteLine(JSONOutput);
         
            JSONOutput = Regex.Replace(JSONOutput, @"\t|\n|\r", "");
            JSONOutput = Regex.Unescape(JSONOutput);
            JSONOutput = Regex.Replace(JSONOutput, @"^""|""$|\\n?", "");
            JSONOutput = JSONOutput.Replace(@"\", @"");
           
            //JSONOutput = JSONOutput.Substring(1, JSONOutput.Length - 1);
            Console.WriteLine(JSONOutput);
            IsBusy = false;

            return await mainC.mVConnector.PostNewMitglied(idGruppe, JSONOutput);
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
