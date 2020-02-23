using BdP_MV.Exceptions;
using BdP_MV.Model.Mitglied;
using BdP_MV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MitgliederDetails.Edit
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewAusbildung : ContentPage
	{
        public NewAusbildungViewModel viewModel;
        int idGruppe;
        Boolean newAusbildung;

        public NewAusbildung ()
		{
			InitializeComponent ();
		}
        public NewAusbildung(MitgliedDetails mitglied)
        {
            newAusbildung = true;
            viewModel = new NewAusbildungViewModel(mitglied);
            InitializeComponent();
        }
        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
            }

            catch (NewLoginException ex)
            {
                await DisplayAlert("Fehler", "Deine Sitzung ist abgelaufen. Bitte logge dich neu in die App ein.", "OK");
                Navigation.InsertPageBefore(new LoginForms.Login(), this);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await Navigation.PopAsync();

            }
            catch (WebException ex)
            {
                await DisplayAlert("Fehler", "Fehler beim Herstellen der Internetverbindung", "OK");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                btn_save.IsEnabled = true;



            }
            catch (NoRightsException ex)
            {
                await DisplayAlert("Fehler", "Für diesen Vorgang hast du keine Rechte.", "OK");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            catch (NotAllRequestedFieldsFilledException ex)
            {
                await DisplayAlert("Fehler", ex.Message, "OK");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                btn_save.IsEnabled = true;


            }
            catch (NullReferenceException ex)
            {
                await DisplayAlert("Fehler", ex.Message, "OK");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                btn_save.IsEnabled = true;


            }

        }
        private void fillFelder()
        {

        }
        public async Task LoadPreferences()
        {
            await viewModel.loadSelectableItems();
            kursPicker.ItemsSource = viewModel.bausteine;
            
            if (!newAusbildung)
            {
                fillFelder();
            }
            



        }
        async void kursdatumTooltipKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Datum", "Abschluss des Kurses", "OK");
        }
        async void abwKursnameTooltipKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Abweichender Kursname", "Wenn abweichender Name des Kurses (z.B. Kalu, Tilop oder KfS 2)", "OK");
        }
        async void veranstalterTooltipKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Veranstalter", "z.B. Region Nord (Grundkurs) oder LV Bayern (bei Teilnahme in anderem LV)", "OK");
        }

    }
}