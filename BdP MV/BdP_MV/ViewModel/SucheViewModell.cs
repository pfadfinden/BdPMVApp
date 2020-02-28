using BdP_MV.Model;
using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BdP_MV.ViewModel
{
    public class SucheViewModell : BaseNavigationViewModel
    {

        public MainController mainc;
        public Boolean nurAktiv { get; set; }
        public SearchObject suche { get; set; }

        public SucheViewModell()
        {

        }
        public SucheViewModell(MainController mainco)
        {
            mainc = mainco;
            suche = new SearchObject();
            nurAktiv = true;
        }
        public async Task<List<Mitglied>> SuchDuApp()
        {
           
            if (nurAktiv)
            {
                suche.mglStatusId = "AKTIV";
            }
            else
            {
                suche.mglStatusId = "";
            }

            List<Mitglied> mitglieder = await mainc.mitgliederController.MitgliederAbrufenBySearch(suche);
          
            return mitglieder;

        }
        public async Task<ItemDetailViewModel> mitgliedDetailsVorladen(int idMitglied, int idGruppe)

        {
            ItemsViewModel ivm = new ItemsViewModel(mainc);

            return await ivm.mitgliedDetailsVorladen(idMitglied, idGruppe);




        }
    }
}
