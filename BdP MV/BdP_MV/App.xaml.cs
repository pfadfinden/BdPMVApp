using BdP_MV.Services;
using BdP_MV.View.LoginForms;

using Xamarin.Forms;

namespace BdP_MV
{
    public partial class App : Application
    {
        public App()
        {
            MainController mainC = new MainController();
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new Login();
            else
                MainPage = new NavigationPage(new Login());
        }
    }
}