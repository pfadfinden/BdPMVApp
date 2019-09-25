using BdP_MV.Model.Settings;
using BdP_MV.Services;
using BdP_MV.ViewModel;
using Newtonsoft.Json;
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
		
            viewModel = new SettingsViewModel();
            BindingContext = viewModel;
            InitializeComponent();
            try
            {
                viewModel.Sortierreihenfolge = ((List<SettingKeyValue>)viewModel.Sortierreihenfolgen).FirstOrDefault(c => c.Value == viewModel.sortierreihenfolge);
                reihenfolgePicker.SelectedItem = viewModel.Sortierreihenfolge;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            showInaktiv.IsToggled = viewModel.inaktiveAnzeigen;
            kleingruppenload.IsToggled = viewModel.loadKleingruppen;

        }
        public async void reihenfolgeChanged(object sender, EventArgs e)
        {
            try
            {
                SettingKeyValue reihenfolge = (SettingKeyValue)reihenfolgePicker.SelectedItem;

                viewModel.sortierreihenfolge = reihenfolge.Value;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            viewModel.EinstellungenAnwenden();
        }

        public async void kleingruppenChanged(object sender, EventArgs e)
        {
            viewModel.loadKleingruppen = kleingruppenload.IsToggled;
            viewModel.EinstellungenAnwenden();
        }
        public async void inaktivAnzeigenChanged(object sender, EventArgs e)
        {
            viewModel.inaktiveAnzeigen = showInaktiv.IsToggled;
            viewModel.EinstellungenAnwenden();


        }
        public async void gruppenNeuLaden(object sender, EventArgs e)
        {
            MainController mainc = new MainController();
            await mainc.groupControl.AlleGruppenAbrufen(0, "");
            Application.Current.Properties["Gruppen"] = JsonConvert.SerializeObject(mainc.groupControl.alleGruppen);
            Application.Current.Properties["lastGroupCall"] = DateTime.Now;
            await Application.Current.SavePropertiesAsync();


        }
    }
}