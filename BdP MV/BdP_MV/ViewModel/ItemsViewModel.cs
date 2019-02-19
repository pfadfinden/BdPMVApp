using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BdP_MV.Exceptions;
using BdP_MV.Model.Mitglied;
using BdP_MV.Model;
using BdP_MV.Services;
using MvvmHelpers;
using Xamarin.Forms;

namespace BdP_MV.ViewModel
{
    public class ItemsViewModel : BaseNavigationViewModel
    {
        public MainController mainC;
        public Gruppe aktGruppe { get; set; }
        public List<Mitglied> ausgewaehlteMitglieder { get; set; }
        public ItemsViewModel(MainController mainCo)
        {
            mainC = mainCo;


          
            ausgewaehlteMitglieder = new List<Mitglied>();
            Mitglied test = new Mitglied();
            test.ansprechname = "test";
            ausgewaehlteMitglieder.Add(test);
            


        }
        
        public async Task MitgliederAusGruppeLaden()
        {
            
            mainC.einsteillungen.aktuelleGruppe = aktGruppe.id;
            await Task.Run(async () => await mainC.mitgliederController.MitgliederAktualisierenByGroup());

            ausgewaehlteMitglieder = mainC.mitgliederController.AktiveMitglieder;
            
        }
        public async Task<ItemDetailViewModel> mitgliedDetailsVorladen(int idMitglied)
        {
            return await mitgliedDetailsVorladen(idMitglied, aktGruppe.id);
        }
        public async Task<ItemDetailViewModel> mitgliedDetailsVorladen(int idMitglied, int idGruppe)
        
            {
            Task<List<SGB8>> task_sgb8 = mainC.mitgliederController.Sgb8Abrufen(idMitglied);
            Task<List<Ausbildung>> task_ausbildung = mainC.mitgliederController.AusbildungenAbrufen(idMitglied);
            Task<List<Taetigkeit>> task_taetigkeiten = mainC.mitgliederController.TaetigkeitenAbrufen(idMitglied);
            MitgliedDetails mitgliedDetails = await Task.Run(async () => await mainC.mitgliederController.MitgliedDetailsAbrufen(idMitglied, idGruppe)); 
            ItemDetailViewModel viewModelMitgliedDetails = new ItemDetailViewModel(mitgliedDetails, mainC);
            viewModelMitgliedDetails.sgb8 = await task_sgb8 ;
            viewModelMitgliedDetails.taetigkeiten = await task_taetigkeiten;
            Task nachbarbeitung = viewModelMitgliedDetails.Nachbearbeitung();
                      
            viewModelMitgliedDetails.ausbildung = await task_ausbildung;
            await nachbarbeitung;

            return viewModelMitgliedDetails;




        }

    }
}

