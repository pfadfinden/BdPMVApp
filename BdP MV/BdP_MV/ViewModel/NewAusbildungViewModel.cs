using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.ViewModel
{
    public class NewAusbildungViewModel : BaseNavigationViewModel
    {
        public MainController mainC;
        public List<SelectableItem> bausteine;

        public int mitgliedID;
        private int gruppierungsID;
        public Ausbildung_Details ausbildung;

        public NewAusbildungViewModel(Ausbildung_Details ausbildungsdetails, MitgliedDetails mitgliedDetail)
        {
            mainC = new MainController();
            ausbildung = ausbildungsdetails;
            mitgliedID = mitgliedDetail.id;
            gruppierungsID = mitgliedDetail.gruppierungId;


        }
        public NewAusbildungViewModel(MitgliedDetails mitgliedDetail)
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
    }
}
