using BdP_MV.Exceptions;
using BdP_MV.ViewModel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BdP_MV.View.MitgliederDetails
{
    public partial class NewMitglied : ContentPage
    {

        public NewMitgliedViewModel viewModel;
        int idGruppe;



        public NewMitglied(int idgrp)
        {
            //   InitializeComponent();
            // this.viewModel = new NewMitgliedViewModel(new Services.MainController());
            viewModel = new NewMitgliedViewModel(idgrp);

            idGruppe = idgrp;
            InitializeComponent();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                viewModel.mitglied.vorname = vornameEntry.Text;
                viewModel.mitglied.nachname = nachnameEntry.Text;
                viewModel.mitglied.spitzname = spitznameEntry.Text;
                viewModel.mitglied.geburtsDatum = geburtsdatumEntry.Date;
                viewModel.mitglied.eintrittsdatum = eintrittsdatumEntry.Date;
                viewModel.mitglied.email = email.Text;
                viewModel.mitglied.zeitschriftenversand = zeitschriftenversand.IsToggled;
                viewModel.mitglied.emailVertretungsberechtigter = emailVertretungsberechtigter.Text;
                viewModel.mitglied.land = ((SelectableItem)landpicker.SelectedItem).descriptor;
                viewModel.mitglied.landId = Convert.ToInt32(((SelectableItem)landpicker.SelectedItem).Id);
                viewModel.mitglied.staatsangehoerigkeit = ((SelectableItem)landpicker.SelectedItem).descriptor;
                viewModel.mitglied.staatsangehoerigkeitId = Convert.ToInt32(((SelectableItem)landpicker.SelectedItem).Id);
                viewModel.mitglied.geschlecht = ((SelectableItem)geschlechtspicker.SelectedItem).descriptor;
                viewModel.mitglied.geschlechtId = Convert.ToInt32(((SelectableItem)geschlechtspicker.SelectedItem).Id);
                viewModel.mitglied.mglType = ((SelectableItem)mitgliedsartpicker.SelectedItem).descriptor;
                viewModel.mitglied.ersteTaetigkeitId = ((SelectableItem)mitgliedsartpicker.SelectedItem).Id;
                viewModel.mitglied.beitragsart = ((SelectableItem)beitragsartpicker.SelectedItem).descriptor;
                viewModel.mitglied.beitragsartId = Convert.ToInt32(((SelectableItem)beitragsartpicker.SelectedItem).Id);
                viewModel.mitglied.strasse = strasse.Text;
                viewModel.mitglied.plz = plz.Text;
                viewModel.mitglied.ort = ort.Text;
                viewModel.mitglied.nameZusatz = nameZusatz.Text;
                viewModel.mitglied.telefon1 = telefon1.Text;
                viewModel.mitglied.telefon3 = telefon3.Text;
                await viewModel.GenerateJSON(idGruppe);
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
                

            }
        }
        public async Task LoadPreferences()
        {
            await viewModel.loadSelectableItems();
            geschlechtspicker.ItemsSource = viewModel.geschlechter;
            landpicker.ItemsSource = viewModel.land;
            beitragsartpicker.ItemsSource = viewModel.beitragsart;
            mitgliedsartpicker.ItemsSource = viewModel.mitgltype;


        }
    }
}
