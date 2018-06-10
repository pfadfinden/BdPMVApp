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
        public List<Item> Geschlechter { get; private set; }
        public List<Item> Laender { get; private set; }
        public List<Item> Beitragsart { get; private set; }
        public List<Item> Mitgliedstyp { get; private set; }
        public List<Item> Zahlungsart { get; private set; }
        public int id_Gruppe { get; private set; }
        private MainController mainc;
        public MitgliedDetails mitglied { get; set; }

        public EditMitglied(MainController mainCo, int idGruppe, int idMitglied)
        {
            mainc = mainCo;
            id_Gruppe = idGruppe;


        }
        public EditMitglied(MainController mainCo, int idGruppe)

        {

            
        }
        public async Task LoadItems()
        {
            Task<List<Item>> loadGeschlechter = mainc.mVConnector.GetItems("baseadmin/geschlecht/");
            Task<List<Item>> loadLaender = mainc.mVConnector.GetItems("baseadmin/land");
            Task<List<Item>> loadBeitragsart = mainc.mVConnector.GetItems("namiBeitrag/beitragsartmgl/gruppierung/"+id_Gruppe);
            Task<List<Item>> loadMitgliedstyp = mainc.mVConnector.GetItems("nami/enum/mgltype");
            Task<List<Item>> loadZahlart = mainc.mVConnector.GetItems("baseadmin/zahlungskondition/");
            Geschlechter = await loadGeschlechter;
            Laender = await loadLaender;
            Beitragsart = await loadBeitragsart;
            Zahlungsart = await loadZahlart;
        }

    }
}
