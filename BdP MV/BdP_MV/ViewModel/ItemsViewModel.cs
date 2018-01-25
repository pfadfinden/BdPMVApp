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
        public List<Gruppe> alleGruppen {get; set; }
        public Gruppe aktGruppe { get; set; }
        public ItemsViewModel(MainController mainCo)
        {
            mainC = mainCo;


            Task task = GruppenLaden();
            task.Wait();
            alleGruppen = mainC.groupControl.alleGruppen;


        }
        private async Task GruppenLaden()
        {
            IsBusy = true;
           await mainC.groupControl.AlleGruppenAbrufen(0);
            alleGruppen = mainC.groupControl.alleGruppen;
            IsBusy = false;
        }
        public async Task MitgliederAusGruppeLaden()
        {
            IsBusy = true;
            mainC.einsteillungen.aktuelleGruppe = aktGruppe.id;
            await mainC.mitgliederController.MitgliederAktualisierenByGroup();
            IsBusy = false;
        }
    }
}

