using System;
using Xamarin.Forms;
using BdP_MV.ViewModel;
using BdP_MV.Services;
using System.Collections.Generic;
using BdP_MV.Model;
using Xamarin.Forms.Xaml;
using System.Threading.Tasks;

namespace BdP_MV.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
	{
        protected ItemsViewModel viewModel;


		public ItemsPage(MainController mainCo)
		{
            
            
            InitializeComponent();
            viewModel = new ItemsViewModel(mainCo);
            BindingContext = viewModel;
            
            
            




        }
        public async void thePickerSelectedIndexChanged(object sender, EventArgs e)
        {
            //await DisplayAlert("Ausgewählte Gruppe", viewModel.aktGruppe.id.ToString(), "OK");//Method call every time when picker selection changed
            await Task.Run(async () => await this.viewModel.MitgliederAusGruppeLaden());
            MitgliedView.ItemsSource = viewModel.ausgewaehlteMitglieder;
            
            
        }

        /// <summary>
        /// The action to take when a list item is tapped.
        /// </summary>
        /// <param name="sender"> The sender.</param>
        /// <param name="e">The ItemTappedEventArgs</param>
        async void ItemTapped(object sender, ItemTappedEventArgs e)
        {
            IsBusy = true;
            Mitglied selected = (Mitglied)MitgliedView.SelectedItem;
            int selectedId = selected.id;
            MitgliedDetails mitDetails = await viewModel.mainC.mitgliederController.MitgliedDetailsAbrufen(selectedId);
            await Navigation.PushAsync(new ItemDetailPage( new ItemDetailViewModel(mitDetails, viewModel.mainC)));

            // prevents the list from displaying the navigated item as selected when navigating back to the list
            ((ListView)sender).SelectedItem = null;
            IsBusy=false;
        }

        /// <summary>
        /// The action to take when the + ToolbarItem is clicked on Android.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The EventArgs</param>
        void AndroidAddButtonClicked(object sender, EventArgs e)
		{
			
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();


            // The navigation logic startup needs to diverge per platform in order to meet the UX design requirements
            
        }
    }
}

