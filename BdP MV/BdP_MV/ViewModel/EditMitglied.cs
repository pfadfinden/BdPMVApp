using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.ViewModel
{
    public class EditMitglied

    {
        public List<SelectableItem> Geschlechter { get; private set; }   //https://qa.mv.meinbdp.de/ica/rest/baseadmin/geschlecht
        public List<SelectableItem> Staatsangehoerigkeit { get; private set; }  //https://qa.mv.meinbdp.de/ica/rest/baseadmin/staatsangehoerigkeit
        public List<SelectableItem> Laender { get; private set; }  //https://qa.mv.meinbdp.de/ica/rest/baseadmin/land/
        public List<SelectableItem> Beitragsart { get; private set; } //https://qa.mv.meinbdp.de/ica/rest/namiBeitrag/beitragsartmgl/gruppierung/448/
        public List<SelectableItem> Mitgliedstyp { get; private set; } //https://qa.mv.meinbdp.de/ica/rest/nami/enum/mgltype
        public List<SelectableItem> Zahlungsart { get; private set; } //https://qa.mv.meinbdp.de/ica/rest/baseadmin/zahlungskondition/
        public int id_Gruppe { get; private set; }
        private MainController mainc;
        public MitgliedDetails mitglied { get; set; }
        public SelectableItem SelectedGeschlecht { get; set; }
        public SelectableItem SelectedBeitragsart { get; set; }
        private Boolean neuesMitglied;

        public EditMitglied(MainController mainCo, int idGruppe, int idMitglied)
        {
            mainc = mainCo;
            id_Gruppe = idGruppe;
            neuesMitglied = true;



        }
        public EditMitglied(MainController mainCo, int idGruppe)

        {
            neuesMitglied = false;
            
            
        }
        public async Task LoadItems()
        {
            Task<List<SelectableItem>> loadGeschlechter = mainc.mVConnector.GetItems("baseadmin/geschlecht/");
            Task<List<SelectableItem>> loadLaender = mainc.mVConnector.GetItems("baseadmin/land");
            Task<List<SelectableItem>> loadBeitragsart = mainc.mVConnector.GetItems("namiBeitrag/beitragsartmgl/gruppierung/"+id_Gruppe);
            Task<List<SelectableItem>> loadMitgliedstyp = mainc.mVConnector.GetItems("nami/enum/mgltype");
            Task<List<SelectableItem>> loadZahlart = mainc.mVConnector.GetItems("baseadmin/zahlungskondition/");
            Geschlechter = await loadGeschlechter;
            Laender = await loadLaender;
            Beitragsart = await loadBeitragsart;
            Zahlungsart = await loadZahlart;
        }


    }
}
