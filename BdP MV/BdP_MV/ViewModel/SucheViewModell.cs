using BdP_MV.Model;
using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.ViewModel
{
    public class SucheViewModell : BaseNavigationViewModel
    {
        public MainController mainc;
        public SearchObject suche  { get; set; }

    public SucheViewModell()
        {

        }
        public SucheViewModell (MainController mainCo)
        {
            mainc = mainCo;
            suche = new SearchObject();
        }
        public async Task<List<Mitglied>> SuchDuApp()
        {
           

            List<Mitglied> mitglieder = await mainc.mitgliederController.MitgliederAbrufenBySearch(suche);
            return mitglieder;
            
        }
    }
}
