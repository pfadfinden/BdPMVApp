using BdP_MV.Services;
using BdP_MV.ViewModel;
using System;
using System.Globalization;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace BdP_MV.View.LoginForms

{
    //[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ForgotPW : ContentPage
    {
        private ForgotPWViewModel viewModel;
        public ForgotPW(MainController mainCo)
        {

            InitializeComponent();

            viewModel = new ForgotPWViewModel(mainCo);

            BindingContext = viewModel;


        }
        async void OnPWLostButtonClicked(object sender, EventArgs e)
        {
            IsBusy = true;
            try
            {
                DateTime test = birthdate.Date;
                CultureInfo ci = new CultureInfo("de-DE");
                string geburtsdatum = test.ToString("d", ci);
                Boolean isValid = false;
                String response = await Task.Run(async () => await viewModel.resetPW(usernameEntry.Text, geburtsdatum, emailEntry.Text));
                if (String.IsNullOrEmpty(response))
                {
                    isValid = true;
                }
                if (isValid)
                {
                    Navigation.InsertPageBefore(new Login(), this);
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Fehler bei der Anmeldung", response, "OK");
                }
            }
            catch (Exception)
            {

            }
            IsBusy = false;
        }

    }
}