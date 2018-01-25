using System;
using Xamarin.Forms;
using BdP_MV.ViewModel;
using BdP_MV.Services;
using System.Collections.Generic;
using BdP_MV.Model;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ItemsPage : ContentPage
	{
        protected ItemsViewModel viewModel;
        private List<Gruppe> alleGruppenView;


		public ItemsPage(MainController mainCo)
		{
            
            
            InitializeComponent();
            viewModel = new ItemsViewModel(mainCo);
            BindingContext = viewModel;




        }

        /// <summary>
        /// The action to take when a list item is tapped.
        /// </summary>
        /// <param name="sender"> The sender.</param>
        /// <param name="e">The ItemTappedEventArgs</param>
        void ItemTapped(object sender, ItemTappedEventArgs e)
		{
			//Navigation.PushAsync(new ItemDetailPage() { BindingContext = new ItemDetailViewModel((Acquaintance)e.Item) });

            // prevents the list from displaying the navigated item as selected when navigating back to the list
			((ListView)sender).SelectedItem = null;
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

