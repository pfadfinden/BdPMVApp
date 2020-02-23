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
        public NewAusbildung(Ausbildung_Details ausbildung, MitgliedDetails mitglied)
        {
            newAusbildung = false;
            viewModel = new NewAusbildungViewModel(ausbildung, mitglied);
            InitializeComponent();
            this.Title = "Ausbildung bearbeiten";

        }
        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (kursPicker.SelectedIndex == -1)
                {
                    throw new NotAllRequestedFieldsFilledException("Die Kursauswahl ist ein Pflichtfeld.");
                }

                if (kursdatumEntry.Date == DateTime.Today)
                {
                    bool answer = await DisplayAlert("Kursdatum", "Ist der Kurs wirklich heute zuende gegangen?", "Ja", "Nein");
                    if (!answer)
                    {
                        throw new NotAllRequestedFieldsFilledException("Bitte das Kursdatum korrigieren.");
                    }
                }
                viewModel.ausbildung.veranstalter = veranstalterEntry.Text;
                viewModel.ausbildung.vstgName = abwKursnameEntry.Text;
                viewModel.ausbildung.vstgTag = kursdatumEntry.Date;
                viewModel.ausbildung.bausteinId = Convert.ToInt32(((SelectableItem)kursPicker.SelectedItem).Id);
                viewModel.ausbildung.baustein = ((SelectableItem)kursPicker.SelectedItem).descriptor;


                btn_save.IsEnabled = false;
                viewModel.IsBusy = true;
                String response;
                if (newAusbildung)
                {
                    response = await viewModel.CreateNewAusbildung();
                    await DisplayAlert("Ausbildung erfolgreich angelegt!", response, "OK");

                }
                else
                {
                    response = await viewModel.UpdateExistingAusbildung();
                    await DisplayAlert("Ausbildung erfolgreich geändert!", response, "OK");

                }
                viewModel.IsBusy = false;

                await Navigation.PopAsync();

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
            veranstalterEntry.Text = viewModel.ausbildung.veranstalter;
            abwKursnameEntry.Text= viewModel.ausbildung.vstgName;
            
            try
            {
                kursPicker.SelectedItem = ((List<SelectableItem>)viewModel.bausteine).FirstOrDefault(c => c.Id == viewModel.ausbildung.bausteinId.ToString());
                kursdatumEntry.Date = (DateTime)viewModel.ausbildung.vstgTag;
            }
            catch (Exception e)
            {

            }
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