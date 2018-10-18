using BdP_MV.Exceptions;
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
            IsBusy = true;
            try
            {
                viewModel.suche.vorname = firstnameEntry.Text;
                viewModel.suche.nachname = lastnameEntry.Text;
                viewModel.suche.mglWohnort = wohnortEntry.Text;
                viewModel.suche.spitzname = nicknameEntry.Text;
                viewModel.suche.alterVon = ageFromEntry.Text;
                viewModel.suche.alterBis = ageToEntry.Text;
                List<Mitglied> mitglieder = await Task.Run(async () => await viewModel.SuchDuApp());
                await Navigation.PushAsync(new ItemsPage(viewModel.mainc, mitglieder));

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

            IsBusy = false;
        }
    }
}