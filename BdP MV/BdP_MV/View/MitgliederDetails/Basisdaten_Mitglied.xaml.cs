using BdP_MV.View.MitgliederDetails.Edit;
using BdP_MV.ViewModel;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MitgliederDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Basisdaten_Mitglied : ContentPage
    {
        ItemDetailViewModel viewModel;

        public Basisdaten_Mitglied()
        {
            InitializeComponent();
        }
        public Basisdaten_Mitglied(ItemDetailViewModel viewModel)
        {

            InitializeComponent();
            BindingContext = this.viewModel = viewModel;


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