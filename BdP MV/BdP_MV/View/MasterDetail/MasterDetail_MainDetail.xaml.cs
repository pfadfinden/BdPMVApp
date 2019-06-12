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
    public partial class MasterDetail_MainDetail : ContentPage
    {
        String htmlString;

        public MasterDetail_MainDetail()
        {
            InitializeComponent();
            try
            {
                htmlString = (String)App.Current.Properties["news"];
                HTMLabel.Text = htmlString;
            }
            catch (Exception e)
            {
            }
        }
    }
}