using BdP_MV.View.MitgliederDetails.Edit;
using BdP_MV.ViewModel;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BdP_MV.View.MitgliederDetails
{
    public partial class MitgliederStammDaten : ContentPage
    {
        ItemDetailViewModel viewModel;

        // Note - The Xamarin.Forms Previewer requires a default, parameterless constructor to render a page.
        public MitgliederStammDaten()
        {
            InitializeComponent();


        }

        public MitgliederStammDaten(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Typically, is preferable to call into the viewmodel for OnAppearing() logic to be performed,
            // but we're not doing that in this case because we need to interact with the Xamarin.Forms.Map property on this Page.
            // In the future, the Map type and it's properties may get more binding support, so that the map setup can be omitted from code-behind.
            // await SetupMap();

        }
        async void NewMitglied_Activated(object sender, EventArgs e)
        {
            viewModel.IsBusy = true;

            NewMitglied neueMitgliesseite = new NewMitglied(viewModel.mitglied);
            await neueMitgliesseite.LoadPreferences();
            viewModel.IsBusy = false;
            await Navigation.PushAsync(neueMitgliesseite);
        }

    }
}
