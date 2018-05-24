using BdP_MV.Exceptions;
using BdP_MV.Model.Mitglied;
using BdP_MV.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MitgliederDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ausbildung_Liste : ContentPage
    {
        private ItemDetailViewModel viewModel;

        public Ausbildung_Liste()
        {
            InitializeComponent();

        }
        public Ausbildung_Liste(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            IsBusy = true;
            try
            {
                CultureInfo ci = new CultureInfo("de-DE");

                Ausbildung selected = (Ausbildung)MyListView.SelectedItem;
                Ausbildung_Details ausbildung_selected_details = await viewModel.mainC.mVConnector.AusbildungDetails(selected.id, viewModel.mitglied.id);

                String details = "Kurs: " + ausbildung_selected_details.baustein;
                DateTime datum;
                datum = (DateTime)ausbildung_selected_details.vstgTag;
                details += "\nKursdatum: " + datum.ToString("d", ci);
                details += "\nMitglied: " + ausbildung_selected_details.mitglied;
                IsBusy = false;
                await DisplayAlert(selected.entries_baustein, ausbildung_selected_details.baustein, "OK");
            }
            catch (NewLoginException ex)
            {
                IsBusy = false;
                await DisplayAlert("Fehler", "Deine Sitzung ist abgelaufen. Bitte logge dich neu in die App ein.", "OK");
                Navigation.InsertPageBefore(new Login(), this);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await Navigation.PopAsync();
                


            }
            catch (WebException ex)

            {
                IsBusy = false;
                await DisplayAlert("Fehler", "Fehler beim Herstellen der Internetverbindung", "OK");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);


            }
            catch (NoRightsException ex)
            {
                IsBusy = false;
                await DisplayAlert("Fehler", "Für diesen Vorgang hast du keine Rechte.", "OK");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
