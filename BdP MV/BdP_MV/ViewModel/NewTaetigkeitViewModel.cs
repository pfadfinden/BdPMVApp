using BdP_MV.Exceptions;
using BdP_MV.Ext_Packages;
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
    public class NewTaetigkeitViewModel : BaseNavigationViewModel
    {
        public MainController mainC;
        public List<SelectableItem> bausteine;

        public int mitgliedID;
        private int gruppierungsID;
        public Ausbildung_Details ausbildung;

        public NewTaetigkeitViewModel(Ausbildung_Details ausbildungsdetails, MitgliedDetails mitgliedDetail)
        {
            mainC = new MainController();
            ausbildung = ausbildungsdetails;
            mitgliedID = mitgliedDetail.id;
            gruppierungsID = mitgliedDetail.gruppierungId;


        }
        public NewTaetigkeitViewModel(MitgliedDetails mitgliedDetail)
        {
            mainC = new MainController();
            mitgliedID = mitgliedDetail.id;
            ausbildung = new Ausbildung_Details();
            gruppierungsID = mitgliedDetail.gruppierungId;

        }
        public async Task loadSelectableItems()
        {
            bausteine = await mainC.mVConnector.GetItems("module/baustein/");
        }
        public async Task<String> UpdateExistingAusbildung()
        {
            IsBusy = true;


                  

            string JSONOutput = JsonConvert.SerializeObject(ausbildung,
                           Newtonsoft.Json.Formatting.None,
                           new JsonSerializerSettings
                           {
                               DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                               ContractResolver = new NullToEmptyStringResolver()

                           });
            
            IsBusy = false;

            return await mainC.mVConnector.PutChangeAusbildung(mitgliedID,ausbildung.id, JSONOutput);
        }
        public async Task<String> CreateNewAusbildung()
        {
            IsBusy = true;


           
           


            string JSONOutput = JsonConvert.SerializeObject(ausbildung,
                           Newtonsoft.Json.Formatting.None,
                           new JsonSerializerSettings
                           {
                               DateTimeZoneHandling = DateTimeZoneHandling.Unspecified,
                               ContractResolver = new NullToEmptyStringResolver()

                           });


            JSONOutput = Regex.Replace(JSONOutput, @"\t|\n|\r", "");
            JSONOutput = Regex.Unescape(JSONOutput);
            JSONOutput = Regex.Replace(JSONOutput, @"^""|""$|\\n?", "");
            JSONOutput = JSONOutput.Replace(@"\", @"");

            //JSONOutput = JSONOutput.Substring(1, JSONOutput.Length - 1);
            string result = await mainC.mVConnector.PostNewAusbildung(mitgliedID, JSONOutput);
            IsBusy = false;

            return result;



        }
    }
}
