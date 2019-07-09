using BdP_MV.Exceptions;
using BdP_MV.Model;
using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using BdP_MV.View.MitgliederDetails;
using BdP_MV.View.MitgliederDetails.Edit;
using BdP_MV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MasterDetail
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ItemsPage : ContentPage
    {
        protected ItemsViewModel viewModel;
        private Boolean searchOrigin;

        


        public ItemsPage(MainController mainCo)
        {

            InitializeComponent();
            searchOrigin = false;
            testpicker.ItemsSource = (List<Gruppe>)App.Current.Properties["Gruppen"];
            
            


            viewModel = new ItemsViewModel(mainCo);

            BindingContext = viewModel;

            //  Task.Run(async () => await loadGroupStaff()).Wait();
            //  Console.WriteLine(viewModel.mainC.groupControl.alleGruppen.ToString());

        }
        public ItemsPage(MainController mainCo, List<Mitglied> mitgliederliste)
        {
            searchOrigin = true;

            InitializeComponent();

            viewModel = new ItemsViewModel(mainCo);

            testpicker.IsVisible = false;

            BindingContext = viewModel;
            viewModel.ausgewaehlteMitglieder = mitgliederliste;
            MitgliedView.ItemsSource = viewModel.ausgewaehlteMitglieder;
            this.Title = "Suchergebnis";


        }



       
        public async void thePickerSelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                //await DisplayAlert("Ausgewählte Gruppe", viewModel.aktGruppe.id.ToString(), "OK");//Method call every time when picker selection changed


                this.IsBusy = true;
                viewModel.aktGruppe = (Gruppe)testpicker.SelectedItem;

                await this.viewModel.MitgliederAusGruppeLaden();
                ImageNewButton.IsVisible = viewModel.isNewMitgliedEnabled;
                MitgliedView.ItemsSource = viewModel.ausgewaehlteMitglieder;
                this.IsBusy = false;
                
                
                

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
            IsBusy = false;


        }

        /// <summary>
        /// The action to take when a list item is tapped.
        /// </summary>
        /// <param name="sender"> The sender.</param>
        /// <param name="e">The ItemTappedEventArgs</param>
        async void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            IsBusy = true;
            try
            {
                IsBusy = true;


                Mitglied selected = (Mitglied)MitgliedView.SelectedItem;
                int selectedId = selected.id;

                if (searchOrigin)
                {
                    await Navigation.PushAsync(new MitgliederDetails.TabbedMitgliederDetails(await viewModel.mitgliedDetailsVorladen(selectedId, selected.entries_gruppierungId)));
                }
                else
                {
                    await Navigation.PushAsync(new MitgliederDetails.TabbedMitgliederDetails(await viewModel.mitgliedDetailsVorladen(selectedId)));
                }
                // prevents the list from displaying the navigated item as selected when navigating back to the list
                ((ListView)sender).SelectedItem = null;
                IsBusy = false;
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
            IsBusy = false;

        }

        /// <summary>
        /// The action to take when the + ToolbarItem is clicked on Android.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The EventArgs</param>
        async void NewMitglied_Activated(object sender, EventArgs e)
        {
                viewModel.IsBusy = true;

                NewMitglied neueMitgliesseite = new MitgliederDetails.Edit.NewMitglied(viewModel.aktGruppe.id);
                await neueMitgliesseite.LoadPreferences();
                viewModel.IsBusy = false;
                await Navigation.PushAsync(neueMitgliesseite);
        }


    }
}