using System;
using Xamarin.Forms;
using BdP_MV.ViewModel;
using BdP_MV.Services;
using System.Collections.Generic;
using BdP_MV.Model.Mitglied;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;
using BdP_MV.Exceptions;
using System.Net;
using Xamarin.Forms.Internals;


namespace BdP_MV.View
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
            testpicker.IsEnabled = false;
            testpicker.IsVisible = false;
            BindingContext = viewModel;
            viewModel.ausgewaehlteMitglieder = mitgliederliste;
                            MitgliedView.ItemsSource = viewModel.ausgewaehlteMitglieder;
            this.Title = "Suchergebnis";


        }



        public async void thePickerFocused(object sender, EventArgs e)
        {
            
                IsBusy = true;
                // base.OnAppearing();
                if (viewModel.mainC.groupControl.alleGruppen.Count==0)
                {
                    //Other code etc.
                    try
                    {
                        if (this.viewModel.mainC.groupControl.alleGruppen.Count == 0)
                        {
                            //await Task.Run(async () => await viewModel.GruppenLaden());
                            await viewModel.GruppenLaden();
                            Console.WriteLine(viewModel.mainC.groupControl.alleGruppen.ToString());
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
                    catch (WebException b)
                    {
                        await DisplayAlert("Fehler", "Fehler beim Herstellen der Internetverbindung", "OK");
                        Console.WriteLine(b.Message);
                        Console.WriteLine(b.StackTrace);

                    }
                }
            testpicker.ItemsSource = viewModel.mainC.groupControl.alleGruppen;

            IsBusy = false;

            
        }





            public async void thePickerSelectedIndexChanged(object sender, EventArgs e)
        {
           
            try
            {
                //await DisplayAlert("Ausgewählte Gruppe", viewModel.aktGruppe.id.ToString(), "OK");//Method call every time when picker selection changed
                IsBusy = true;
                await Task.Run(async () => await this.viewModel.MitgliederAusGruppeLaden());
                MitgliedView.ItemsSource = viewModel.ausgewaehlteMitglieder;
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
                    await Navigation.PushAsync(new MitgliederDetails.TabbedMitgliederDetails(await viewModel.mitgliedDetailsVorladen(selectedId,selected.entries_gruppierungId)));
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
        void AndroidAddButtonClicked(object sender, EventArgs e)
		{
			
		}

		
    }
}

