using BdP_MV.Exceptions;
using BdP_MV.Ext_Packages;
using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using BdP_MV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Suche : ContentPage
    {

        SucheViewModell viewModel;
        public Suche()
        {
            InitializeComponent();
        }
        public Suche(MainController mainCo)
        {
            viewModel = new SucheViewModell(mainCo);
            BindingContext = viewModel;
            InitializeComponent();
        }
        async void OnSearchButtonClicked(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;

            //inProgress.IsRunning = true;
            //inProgress.IsEnabled = true;
            //inProgress.IsVisible = true;
            try
            {
                
                viewModel.suche.vorname = firstnameEntry.Text;
                viewModel.suche.nachname = lastnameEntry.Text;
                viewModel.suche.mglWohnort = wohnortEntry.Text;
                viewModel.suche.spitzname = nicknameEntry.Text;
                viewModel.suche.alterVon = ageFromEntry.Text;
                viewModel.suche.alterBis = ageToEntry.Text;
                //List<Mitglied> mitglieder = await Task.Run(async () => await viewModel.SuchDuApp());
                List<Mitglied> mitglieder = await viewModel.SuchDuApp();
                //Da wird die Reaktion gecheckt. Wenn kein Result, dann ist wird eine Meludung geschickt. Bei nur einem Mitglied als Result, wird das Mitglied direkt in der Detailsansicht geladen und bei mehreren Mitgliedern im Result wird die Mitgliedsliste geladen.
                if (mitglieder.Count() == 0)
                {
                    await DisplayAlert("Keine Mitglieder gefunden","Bitte versuch es mit anderen Suchkriterien erneut.", "OK");

                }
                else if (mitglieder.Count() == 1)
                {
                    Mitglied m = mitglieder.First();
                    await Navigation.PushAsync(new MitgliederDetails.TabbedMitgliederDetails(await viewModel.mitgliedDetailsVorladen(m.id, m.entries_gruppierungId)));

                }
                else
                {
                    await Navigation.PushAsync(new ItemsPage(viewModel.mainc, mitglieder)).ConfigureAwait(false);
                }
                       
                
            }
            catch (NewLoginException b)
            {
                await DisplayAlert("Fehler", "Deine Sitzung ist abgelaufen. Bitte logge dich neu in die App ein.", "OK");
                Navigation.InsertPageBefore(new LoginForms.Login(), this);
                Console.WriteLine(b.Message);
                Console.WriteLine(b.StackTrace);
                await Navigation.PopAsync();

            }
            catch (System.Net.WebException b)
            {
                await DisplayAlert("Fehler", "Fehler beim Herstellen der Internetverbindung", "OK");
                Console.WriteLine(b.Message);
                Console.WriteLine(b.StackTrace);

            }
            viewModel.IsBusy = false;
            //inProgress.IsRunning = IsBusy;
            //inProgress.IsEnabled = IsBusy;
            //inProgress.IsEnabled = IsVisible;
        }
    }
}