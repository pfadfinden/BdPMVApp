using BdP_MV.Model.Settings;
using BdP_MV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MasterDetail
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Einstellungen : ContentPage
	{
        private SettingsViewModel viewModel;
		public Einstellungen ()
		{
			InitializeComponent ();
            viewModel = new SettingsViewModel();
            BindingContext = viewModel;
            kleingruppenload.IsToggled = viewModel.settings.loadKleingruppen;
            reihenfolgePicker.SelectedItem = ((List<SettingKeyValue>)viewModel.Sortierreihenfolgen).FirstOrDefault(c => c.Value == viewModel.settings.sortierreihenfolge);


        }

        public async void settingsChanged(object sender, EventArgs e)
        {
            
           SettingKeyValue reihenfolge=(SettingKeyValue)reihenfolgePicker.SelectedItem;
            viewModel.settings.sortierreihenfolge = reihenfolge.Value;
            viewModel.settings.loadKleingruppen = kleingruppenload.IsToggled;
            viewModel.EinstellungenAnwenden();

        }
    }
}