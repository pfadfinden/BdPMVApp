using BdP_MV.Exceptions;
using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
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
        Boolean newMitglied;



        public NewMitglied(int idgrp)
        {
            //   InitializeComponent();
            // this.viewModel = new NewMitgliedViewModel(new Services.MainController());
            viewModel = new NewMitgliedViewModel(idgrp);

            idGruppe = idgrp;
            newMitglied = true;
            InitializeComponent();
        }
        public NewMitglied(MitgliedDetails mitglied)
        {
            //   InitializeComponent();
            // this.viewModel = new NewMitgliedViewModel(new Services.MainController());
            viewModel = new NewMitgliedViewModel(mitglied);
            newMitglied = false;
            fillFelder();
            InitializeComponent();

        }
       private void fillFelder()
        {
            vornameEntry.Text = viewModel.mitglied.vorname;
             nachnameEntry.Text = viewModel.mitglied.nachname;
            spitznameEntry.Text = viewModel.mitglied.spitzname;
            geburtsdatumEntry.Date = (DateTime)viewModel.mitglied.geburtsDatum;
            eintrittsdatumEntry.Date = (DateTime)viewModel.mitglied.eintrittsdatum;
            email.Text = viewModel.mitglied.email;
            zeitschriftenversand.IsToggled = viewModel.mitglied.zeitschriftenversand;
            emailVertretungsberechtigter.Text = viewModel.mitglied.emailVertretungsberechtigter;
            //viewModel.mitglied.landId = Convert.ToInt32(((SelectableItem)landpicker.SelectedItem).Id);
            //viewModel.mitglied.staatsangehoerigkeit = ((SelectableItem)landpicker.SelectedItem).descriptor;
            //viewModel.mitglied.staatsangehoerigkeitId = Convert.ToInt32(((SelectableItem)landpicker.SelectedItem).Id);
            //viewModel.mitglied.geschlecht = ((SelectableItem)geschlechtspicker.SelectedItem).descriptor;
            //viewModel.mitglied.geschlechtId = Convert.ToInt32(((SelectableItem)geschlechtspicker.SelectedItem).Id);
            viewModel.mitglied.beitragsartId = Convert.ToInt32(((SelectableItem)beitragsartpicker.SelectedItem).Id);
            strasse.Text = viewModel.mitglied.strasse;
            plz.Text = viewModel.mitglied.plz;
            ort.Text = viewModel.mitglied.ort;
            nameZusatz.Text = viewModel.mitglied.nameZusatz;
            telefon1.Text = viewModel.mitglied.telefon1;
            telefon3.Text = viewModel.mitglied.telefon3;

        }
        async void GeburtsdatumChanged(object sender, EventArgs e)
        {
            DateTime gebDatum = geburtsdatumEntry.Date;
            int age = viewModel.mainC.mitgliederController.GetAgeFromDate(gebDatum);
            if (age>17 && newMitglied)
            {
                begruendungMitglied.IsVisible = true;
                begruendungMitglied.IsEnabled = true;

                begruendungStamm.IsVisible = true;
                begruendungStamm.IsEnabled = true;

            }
            else
            {
                begruendungMitglied.IsVisible = false;
                begruendungMitglied.IsEnabled = false;

                begruendungStamm.IsVisible = false;
                begruendungStamm.IsEnabled = false;

            }


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
                //viewModel.mitglied.geschlecht = ((SelectableItem)geschlechtspicker.SelectedItem).descriptor;
                viewModel.mitglied.geschlechtId = Convert.ToInt32(((SelectableItem)geschlechtspicker.SelectedItem).Id);
                viewModel.mitglied.mglType = ((SelectableItem)mitgliedsartpicker.SelectedItem).descriptor;
                viewModel.mitglied.ersteTaetigkeitId = ((SelectableItem)mitgliedsartpicker.SelectedItem).Id;
                viewModel.mitglied.beitragsartId = Convert.ToInt32(((SelectableItem)beitragsartpicker.SelectedItem).Id);
                viewModel.mitglied.strasse = strasse.Text;
                viewModel.mitglied.plz = plz.Text;
                viewModel.mitglied.ort = ort.Text;
                viewModel.mitglied.nameZusatz = nameZusatz.Text;
                viewModel.mitglied.telefon1 = telefon1.Text;
                viewModel.mitglied.telefon3 = telefon3.Text;
                viewModel.mitglied.dyn_BegruendungMitglied = begruendungMitglied.Text;
                viewModel.mitglied.dyn_BegruendungStamm = begruendungStamm.Text;
                btn_save.IsEnabled = false;
                IsBusy = true;
                String response = await viewModel.GenerateJSON(idGruppe);
                IsBusy = false;

                await DisplayAlert("Mitglied erfolgreich angelegt!",response, "OK");
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
