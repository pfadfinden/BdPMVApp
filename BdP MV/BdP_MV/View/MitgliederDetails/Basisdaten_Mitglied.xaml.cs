using BdP_MV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MitgliederDetails
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Basisdaten_Mitglied : ContentPage
	{
        ItemDetailViewModel viewModel;
      
        public Basisdaten_Mitglied ()
		{
			InitializeComponent ();
		}
        public Basisdaten_Mitglied(ItemDetailViewModel viewModel)
        {
            
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            if (viewModel.isEditable)
            {
                ImageNewButton.IsVisible = true;
            }
            
          
        }
        async void NewMitglied_Activated(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;

            NewMitglied neueMitgliesseite = new MitgliederDetails.NewMitglied(viewModel.mitglied);
            await neueMitgliesseite.LoadPreferences();
            viewModel.IsBusy = false;
            await Navigation.PushAsync(neueMitgliesseite);
        }
    }
}