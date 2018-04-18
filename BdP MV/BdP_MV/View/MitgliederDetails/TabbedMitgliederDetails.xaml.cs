using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BdP_MV.ViewModel;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MitgliederDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabbedMitgliederDetails : TabbedPage
    {
        string ansprechname;
        public TabbedMitgliederDetails ()
        {
            InitializeComponent();
        }
        public TabbedMitgliederDetails(ItemDetailViewModel viewModel)
        {
            ansprechname = viewModel.mitglied.ansprechname;
            InitializeComponent();
            this.Children.Add(new MitgliederStammDaten(viewModel));
            this.Children.Add(new Basisdaten_Mitglied(viewModel));

        }
       
    }
}