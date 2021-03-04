using BdP_MV.Model;
using BdP_MV.Model;

/* Nicht gemergte Änderung aus Projekt "BdP_MV.iOS"
Vor:
using BdP_MV.Model;
Nach:
using BdP_MV.Model.Mitglied;
*/
using BdP_MV.Services;
using System;

/* Nicht gemergte Änderung aus Projekt "BdP_MV.iOS"
Vor:
using Xamarin.Forms;
Nach:
using System;
using System.Collections.Generic;
using System.Linq;
*/
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace BdP_MV.ViewModel
{
    public class ItemsViewModel : BaseNavigationViewModel
    {
        public bool isNewMitgliedEnabled { get; set; }
        public MainController mainC;
        public Gruppe aktGruppe { get; set; }
        public List<Mitglied> ausgewaehlteMitglieder { get; set; }
        public ItemsViewModel(MainController mainCo)
        {
            mainC = mainCo;
            ausgewaehlteMitglieder = new List<Mitglied>();
            isNewMitgliedEnabled = false;


        }

        public async Task MitgliederAusGruppeLaden()
        {
            IsBusy = true;
            Preferences.Set("aktuelleGruppe", aktGruppe.id);
            await Task.Run(async () => await mainC.mitgliederController.MitgliederAktualisierenByGroup());

            ausgewaehlteMitglieder = mainC.mitgliederController.AktiveMitglieder;
            isNewMitgliedEnabled = await CheckPermissionForNewInGroup(aktGruppe.id);

            IsBusy = false;
        }
        public async Task<Boolean> CheckPermissionForNewInGroup(int idGroup)
        {
            IsBusy = true;
            Boolean newPermitted = await mainC.groupControl.CheckPermissionForNew(idGroup);
            IsBusy = false;
            return newPermitted;

        }

        public async Task<ItemDetailViewModel> mitgliedDetailsVorladen(int idMitglied)
        {

            ItemDetailViewModel detailViewModel = await mitgliedDetailsVorladen(idMitglied, aktGruppe.id);

            return detailViewModel;
        }
        public async Task<ItemDetailViewModel> mitgliedDetailsVorladen(int idMitglied, int idGruppe)

        {

            Task<List<SGB8>> task_sgb8 = mainC.mitgliederController.Sgb8Abrufen(idMitglied);
            Task<List<Ausbildung>> task_ausbildung = mainC.mitgliederController.AusbildungenAbrufen(idMitglied);
            Task<Boolean> task_editable = mainC.groupControl.CheckPermissionForEdit(idGruppe);
            Task<Boolean> task_editableAusbildung = mainC.mitgliederController.CheckPermissionForEditAusbildung(idMitglied);
            Task<List<Taetigkeit>> task_taetigkeiten = mainC.mitgliederController.TaetigkeitenAbrufen(idMitglied);
            MitgliedDetails mitgliedDetails = await Task.Run(async () => await mainC.mitgliederController.MitgliedDetailsAbrufen(idMitglied, idGruppe));
            ItemDetailViewModel viewModelMitgliedDetails = new ItemDetailViewModel(mitgliedDetails, mainC);
            await Task.WhenAll(task_sgb8, task_taetigkeiten);
            viewModelMitgliedDetails.sgb8 = await task_sgb8;
            viewModelMitgliedDetails.taetigkeiten = await task_taetigkeiten;
            Task nachbarbeitung = viewModelMitgliedDetails.Nachbearbeitung();

            viewModelMitgliedDetails.ausbildung = await task_ausbildung;
            viewModelMitgliedDetails.isEditable = await task_editable;
            viewModelMitgliedDetails.isAusbildungEditable = await task_editableAusbildung;

            await nachbarbeitung;

            return viewModelMitgliedDetails;




        }

    }
}

