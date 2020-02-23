using BdP_MV.Exceptions;
using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using BdP_MV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BdP_MV.View.MitgliederDetails.Edit
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
            lbl_begruendungMitglied.IsVisible = false;
            lbl_begruendungStamm.IsVisible = false;
            begruendungStammTooltip.IsVisible = false;
            begruendungMitglied.IsVisible = false;
            begruendungMitgliedToolTip.IsVisible = false;

        }
        public NewMitglied(MitgliedDetails mitglied)
        {
            //   InitializeComponent();
            // this.viewModel = new NewMitgliedViewModel(new Services.MainController());
            viewModel = new NewMitgliedViewModel(mitglied);
            newMitglied = false;
            

            
            InitializeComponent();
            this.Title = "Mitglied " + mitglied.ansprechname + " bearbeiten";
            lbl_begruendungMitglied.IsVisible = false;
            lbl_begruendungStamm.IsVisible = false;
            begruendungStammTooltip.IsVisible = false;
            begruendungMitglied.IsVisible = false;
            begruendungMitgliedToolTip.IsVisible = false;
            lbl_begruendungStamm.IsVisible = false;
            lbl_begruendungMitglied.IsVisible = false;
        }
       private void fillFelder()
        {
            vornameEntry.Text = viewModel.mitglied.vorname;
             nachnameEntry.Text = viewModel.mitglied.nachname;
            spitznameEntry.Text = viewModel.mitglied.spitzname;
            geburtsdatumEntry.Date = (DateTime)viewModel.mitglied.geburtsDatum;
            eintrittsdatumEntry.Date = (DateTime)viewModel.mitglied.eintrittsdatum;
            email.Text = viewModel.mitglied.email;
            email2.Text = viewModel.mitglied.dyn_eMail2;
            zeitschriftenversand.IsToggled = viewModel.mitglied.zeitschriftenversand;
            emailVertretungsberechtigter.Text = viewModel.mitglied.emailVertretungsberechtigter;
            try
            {
                landpicker.SelectedItem = ((List<SelectableItem>)viewModel.land).FirstOrDefault(c => c.Id == viewModel.mitglied.landId.ToString());
                geschlechtspicker.SelectedItem = ((List<SelectableItem>)viewModel.geschlechter).FirstOrDefault(c => c.Id == viewModel.mitglied.geschlechtId.ToString());
                beitragsartpicker.SelectedItem = ((List<SelectableItem>)viewModel.beitragsart).FirstOrDefault(c => c.Id == viewModel.mitglied.beitragsartId.ToString());
            }
            catch (Exception e)
            {
                
            }
            strasse.Text = viewModel.mitglied.strasse;
            plz.Text = viewModel.mitglied.plz;
            ort.Text = viewModel.mitglied.ort;
            nameZusatz.Text = viewModel.mitglied.nameZusatz;
            telefon1.Text = viewModel.mitglied.telefon1;
            telefon2.Text = viewModel.mitglied.telefon2;
            telefon3.Text = viewModel.mitglied.telefon3;
            lbl_mitgliedsartpicker.IsVisible = false;
            mitgliedsartpicker.IsVisible = false;

        }
        async void GeburtsdatumChanged(object sender, EventArgs e)
        {
            DateTime gebDatum = geburtsdatumEntry.Date;
            int age = viewModel.mainC.mitgliederController.GetAgeFromDate(gebDatum);
            if (age>17 && newMitglied)
            {
                begruendungMitglied.IsVisible = true;
                lbl_begruendungMitglied.IsVisible = true;
                //lbl_begruendungMitglied.IsEnabled = true;
                begruendungMitglied.IsEnabled = true;
                //lbl_begruendungStamm.IsEnabled = true;
                lbl_begruendungStamm.IsVisible = true;
                begruendungStamm.IsVisible = true;
                begruendungStamm.IsEnabled = true;
                begruendungStammTooltip.IsVisible = true;
                begruendungMitglied.IsVisible = true;
                begruendungMitgliedToolTip.IsVisible = true;
            }
            else
            {
                lbl_begruendungMitglied.IsVisible = false;

                // lbl_begruendungMitglied.IsEnabled = true;
                begruendungMitglied.IsVisible = false;
                begruendungMitglied.IsEnabled = false;
                //lbl_begruendungStamm.IsEnabled = true;
                lbl_begruendungStamm.IsVisible = false;
                begruendungStamm.IsVisible = false;
                begruendungStamm.IsEnabled = false;
                begruendungStammTooltip.IsVisible = false;
                begruendungMitglied.IsVisible = false;
                begruendungMitgliedToolTip.IsVisible = false;
            }


        }
        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (landpicker.SelectedIndex == -1)
                {
                    throw new NotAllRequestedFieldsFilledException("Das Land ist ein Pflichtfeld.");
                }
                if (geschlechtspicker.SelectedIndex == -1)
                {
                    throw new NotAllRequestedFieldsFilledException("Das Geschlecht ist ein Pflichtfeld.");
                }
                if (beitragsartpicker.SelectedIndex == -1)
                {
                    throw new NotAllRequestedFieldsFilledException("Die Beitragsart ist ein Pflichtfeld.");

                }
                if (eintrittsdatumEntry.Date == DateTime.Today)
                {
                    bool answer = await DisplayAlert("Eintrittsdatum", "Ist das Eintrittsdatum auf dem Aufnahmeantrag wirklich das heutige Datum?", "Ja", "Nein");
                    if (!answer)
                    {
                        throw new NotAllRequestedFieldsFilledException("Bitte das Eintrittsdatum korrigieren.");
                    }
                }
                viewModel.mitglied.vorname = vornameEntry.Text;
                viewModel.mitglied.nachname = nachnameEntry.Text;
                viewModel.mitglied.spitzname = spitznameEntry.Text;
                viewModel.mitglied.geburtsDatum = geburtsdatumEntry.Date;
                viewModel.mitglied.eintrittsdatum = eintrittsdatumEntry.Date;
                viewModel.mitglied.email = email.Text;
                viewModel.mitglied.dyn_eMail2 = email2.Text;
                viewModel.mitglied.zeitschriftenversand = zeitschriftenversand.IsToggled;
                viewModel.mitglied.emailVertretungsberechtigter = emailVertretungsberechtigter.Text;
                viewModel.mitglied.land = ((SelectableItem)landpicker.SelectedItem).descriptor;
                viewModel.mitglied.landId = Convert.ToInt32(((SelectableItem)landpicker.SelectedItem).Id);
                //viewModel.mitglied.geschlecht = ((SelectableItem)geschlechtspicker.SelectedItem).descriptor;
                viewModel.mitglied.geschlechtId = Convert.ToInt32(((SelectableItem)geschlechtspicker.SelectedItem).Id);
                viewModel.mitglied.beitragsartId = Convert.ToInt32(((SelectableItem)beitragsartpicker.SelectedItem).Id);
                viewModel.mitglied.strasse = strasse.Text;
                viewModel.mitglied.plz = plz.Text;
                viewModel.mitglied.ort = ort.Text;
                viewModel.mitglied.nameZusatz = nameZusatz.Text;
                viewModel.mitglied.telefon1 = telefon1.Text;
                viewModel.mitglied.telefon1 = telefon2.Text;
                viewModel.mitglied.telefon3 = telefon3.Text;
                if (newMitglied)
                {
                    if (mitgliedsartpicker.SelectedIndex == -1)
                    {
                        throw new NotAllRequestedFieldsFilledException("Die Mtigliedsart ist ein Pflichtfeld.");
                    }
                    viewModel.mitglied.dyn_BegruendungMitglied = begruendungMitglied.Text;
                    viewModel.mitglied.dyn_BegruendungStamm = begruendungStamm.Text;
                    viewModel.mitglied.mglType = ((SelectableItem)mitgliedsartpicker.SelectedItem).descriptor;
                    viewModel.mitglied.ersteTaetigkeitId = ((SelectableItem)mitgliedsartpicker.SelectedItem).Id;
                    viewModel.mitglied.staatsangehoerigkeit = ((SelectableItem)landpicker.SelectedItem).descriptor;
                    viewModel.mitglied.staatsangehoerigkeitId = Convert.ToInt32(((SelectableItem)landpicker.SelectedItem).Id);

                }
                btn_save.IsEnabled = false;
                IsBusy = true;
                String response;
                if (newMitglied)
                {
                    response = await viewModel.CreateNewMitglied(idGruppe);
                    await DisplayAlert("Mitglied erfolgreich angelegt!", response, "OK");

                }
                else
                {
                    response = await viewModel.UpdateExistingMitglied();
                    await DisplayAlert("Mitglied erfolgreich geändert!", response, "OK");

                }
                IsBusy = false;

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
            if (!newMitglied)
            {
                fillFelder();
            }
            else
            {
                landpicker.SelectedItem = ((List<SelectableItem>)viewModel.land).FirstOrDefault(c => c.Id == "1");

            }



        }
        async void TooltipKlicked(object sender, EventArgs e)
        {
            Console.WriteLine(sender);
            Console.WriteLine("Klicked!!!");

        }

        async void TooltipTitelKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Titel", "Zum Beispiel Dr.", "OK");

        }
        async void TooltipVornameKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Vorname", "Vornamen ohne Zusatz.", "OK");
        }
        async void TooltipNachnameKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Nachname", "Namenszusatz + Nachname \nz.B: von Meier", "OK");
        }
        async void TooltipEintrittsdatumKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Eintrittsdatum", "Unterschriftsdatum des Mitglieds auf dem Aufnahmeantrag.", "OK");
        }
        async void TooltipMitgliedsartKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Mitgliedsart", "Aus der gewählten Mitgliedsart wird automatisch die erste Mitgliedstätigkeit erzeugt. „Zweitmitglied (Doppelmitgliedschaft)“ ist beim Anlegen eines neuen Mitgliedes unzulässig.", "OK");
        }
        async void TooltipStasseKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Straße", "Postrückläufer werden mit einer # als erstes Zeichen der Straße gekennzeichnet", "OK");
        }
        async void TooltipEmail1Klicked(object sender, EventArgs e)
        {
            await DisplayAlert("E-Mail", "Bei Funktionsträger/innen auf Landes- und Bundesebene pfadfinden.de-Adresse, deren private E-Mail-Adresse unter E-Mail 2.", "OK");
        }
        async void TooltipEmail2Klicked(object sender, EventArgs e)
        {
            await DisplayAlert("E-Mail 2", "Bei Funktionsträger/innen auf Landes- und Bundesebene pfadfinden.de-Adresse, private E-Mailadresse. Ggf. Weiterleitungsziel von E-Mail1", "OK");
        }
        async void TooltipTelefonKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Telefon", "Format: 01234 567890 / International: +44 1234 567890.", "OK");
        }
        async void TooltipBegruendungMitgliedKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Begründung Mitglied", "Hier ist die Begründung des Mitglieds lt. Aufnahmeantrag einzutragen.", "OK");
        }
        async void TooltipBegruendungStammKlicked(object sender, EventArgs e)
        {
            await DisplayAlert("Stellungnahme Stamm", "Hier ist die Stellungnahme des Stammes lt. Aufnahmeantrag einzutragen.", "OK");
        }
    }
}
