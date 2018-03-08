using BdP_MV.Services;
using BdP_MV.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View
{
	//[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ForgotPW : ContentPage
	{
        private ForgotPWViewModel viewModel;
        public ForgotPW (MainController mainCo)
		{

            InitializeComponent();

            viewModel = new ForgotPWViewModel(mainCo);

            BindingContext = viewModel;


        }
        async void OnPWLostButtonClicked(object sender, EventArgs e)
        {
            try
            {
                DateTime test = birthdate.Date;
                CultureInfo ci = new CultureInfo("de-DE");
                string geburtsdatum  = test.ToString("d", ci);
                
                String response = await Task.Run(async () => await viewModel.resetPW(usernameEntry.Text, geburtsdatum, emailEntry.Text));

            }
            catch (Exception ex)
            {

            }
        }

    }
}