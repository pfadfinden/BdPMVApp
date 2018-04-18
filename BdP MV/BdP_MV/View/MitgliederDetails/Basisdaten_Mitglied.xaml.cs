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
        string EintrittsdatumString;
        string GeburtsdatumString;
        public Basisdaten_Mitglied ()
		{
			InitializeComponent ();
		}
        public Basisdaten_Mitglied(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;
            GeburtsdatumString = String.Format("{0:d/M/yyyy}", viewModel.mitglied.geburtsDatum);
            EintrittsdatumString = String.Format("{0:d/M/yyyy}", viewModel.mitglied.eintrittsdatum);
        }
    }
}