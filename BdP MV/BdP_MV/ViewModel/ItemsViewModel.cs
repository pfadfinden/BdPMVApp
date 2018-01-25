using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BdP_MV.Model;
using BdP_MV.Services;
using MvvmHelpers;
using Xamarin.Forms;

namespace BdP_MV.ViewModel
{
    public class ItemsViewModel : BaseNavigationViewModel
    {
        private MainController mainC;
        public List<Gruppe> alleGruppen;
        public Gruppe aktGruppe;
        public ItemsViewModel(MainController mainCo)
        {
            mainC = mainCo;
            IsBusy = true;
            Task.Run(async () => await GruppenLaden());
            IsBusy = false;

        }
        private async Task GruppenLaden()
        {
            await Task.Run(async () => await mainC.groupControl.GetAlleGruppen(0));
            alleGruppen = mainC.groupControl.alleGruppen;
        }
    }
}

