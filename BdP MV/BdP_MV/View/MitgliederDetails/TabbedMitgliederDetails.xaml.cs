
/* Nicht gemergte Änderung aus Projekt "BdP_MV.iOS"
Vor:
using System;
Nach:
using BdP_MV.ViewModel;
using System;
*/
using 
/* Nicht gemergte Änderung aus Projekt "BdP_MV.iOS"
Vor:
using BdP_MV.ViewModel;

using Xamarin.Forms;
Nach:
using Xamarin.Forms;
*/
BdP_MV.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MitgliederDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedMitgliederDetails : TabbedPage
    {
        string ansprechname;
        ItemDetailViewModel viewModel;
        public TabbedMitgliederDetails()
        {
            InitializeComponent();
        }
        public TabbedMitgliederDetails(ItemDetailViewModel p_ViewModel)
        {
            ansprechname = p_ViewModel.mitglied.ansprechname;
            InitializeComponent();
            Children.Add(new MitgliederStammDaten(p_ViewModel));
            Children.Add(new Basisdaten_Mitglied(p_ViewModel));
            Children.Add(new Taetigkeiten_Liste(p_ViewModel));
            Children.Add(new Ausbildung_Liste(p_ViewModel));

            viewModel = p_ViewModel;
            BindingContext = viewModel;

        }


    }
}