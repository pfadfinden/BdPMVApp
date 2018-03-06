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
        public MainController mainC;
        public List<Gruppe> alleGruppen {get; set; }
        public Gruppe aktGruppe { get; set; }
        public List<Mitglied> ausgewaehlteMitglieder { get; set; }
        public ItemsViewModel(MainController mainCo)
        {
            mainC = mainCo;


            Task task = GruppenLaden();//TODO: Muss noch Schöner gemacht werden! Läft zzt. syncron
            task.Wait();
            alleGruppen = mainC.groupControl.alleGruppen;
            ausgewaehlteMitglieder = new List<Mitglied>();
            Mitglied test = new Mitglied();
            test.ansprechname = "test";
            ausgewaehlteMitglieder.Add(test);
            


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
            await Task.Run(async () => await mainC.mitgliederController.MitgliederAktualisierenByGroup());

            ausgewaehlteMitglieder = mainC.mitgliederController.AktiveMitglieder;
            IsBusy = false;
        }

    }
}

