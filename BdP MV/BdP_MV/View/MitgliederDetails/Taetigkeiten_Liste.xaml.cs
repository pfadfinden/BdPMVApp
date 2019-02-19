using BdP_MV.Model.Mitglied;
using BdP_MV.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MitgliederDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Taetigkeiten_Liste : ContentPage
    {
        private ItemDetailViewModel viewModel;
        public Taetigkeiten_Liste()
        {
            InitializeComponent();

        }
        public Taetigkeiten_Liste(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            nurAktiv.IsToggled = false;
            BindingContext = this.viewModel = viewModel;

        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;
            Taetigkeit selectedTaetigkeit = (Taetigkeit)MyListView.SelectedItem;
            String status;
            CultureInfo ci = new CultureInfo("de-DE");
            String zeitraum;
            if (selectedTaetigkeit.aktiv)
            {
                status = "aktiv";
                DateTime von;
                von = (DateTime)selectedTaetigkeit.entries_aktivVon;
                zeitraum = "Seit " + von.ToString("d", ci);
            }
            else
            {
                status = "inaktiv";
                DateTime von;
                von = (DateTime)selectedTaetigkeit.entries_aktivVon;
                DateTime bis;
                bis = (DateTime)selectedTaetigkeit.entries_aktivBis;

                zeitraum = "Von " + von.ToString("d", ci)+ " bis "+ bis.ToString("d", ci);
            }
            String infos = "Untergliederung: " + selectedTaetigkeit.entries_gruppierung + "\nStatus: " + status + "\n" + zeitraum ;
            if (!string.IsNullOrWhiteSpace(selectedTaetigkeit.entries_untergliederung))
                infos += "\nBereich: " + selectedTaetigkeit.entries_untergliederung;

             await DisplayAlert(selectedTaetigkeit.entries_taetigkeit, infos, "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
        void nurAktiv_Toggled(object sender, ToggledEventArgs e)
        {
            if (nurAktiv.IsToggled)
            { MyListView.ItemsSource = viewModel.taetigkeitenAktiv; }
            else
            { MyListView.ItemsSource = viewModel.taetigkeiten; }
        }
    }
}
