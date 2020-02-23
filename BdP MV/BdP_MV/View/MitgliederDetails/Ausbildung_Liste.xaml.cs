using BdP_MV.Exceptions;
using BdP_MV.Model.Mitglied;
using BdP_MV.View.MitgliederDetails.Edit;
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
        async void newAusbildung_Activated(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;

            NewAusbildung neueAusbidlungsseite = new NewAusbildung(viewModel.mitglied);
            await neueAusbidlungsseite.LoadPreferences();
            viewModel.IsBusy = false;
            await Navigation.PushAsync(neueAusbidlungsseite);
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
                Ausbildung_Details ausbildung_selected_details = await viewModel.getAusbildungDetails(selected.id);

                String details = "Kurs: " + ausbildung_selected_details.baustein;
                DateTime datum;
                datum = (DateTime)ausbildung_selected_details.vstgTag;
                details += "\nKursdatum: " + datum.ToString("d", ci);
                
                if (!string.IsNullOrWhiteSpace(ausbildung_selected_details.vstgName))
                {
                    details += "\nAbweichender Kursname: " + ausbildung_selected_details.vstgName;
                }
                if (!string.IsNullOrWhiteSpace(ausbildung_selected_details.veranstalter))
                {
                    details += "\nVeranstalter: " + ausbildung_selected_details.veranstalter;
                }
                IsBusy = false;
                await DisplayAlert(selected.entries_baustein, details, "OK");
            }
            catch (NewLoginException ex)
            {
                IsBusy = false;
                await DisplayAlert("Fehler", "Deine Sitzung ist abgelaufen. Bitte logge dich neu in die App ein.", "OK");
                Navigation.InsertPageBefore(new LoginForms.Login(), this);
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
