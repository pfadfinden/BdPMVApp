using BdP_MV.Services;
using BdP_MV.View.LoginForms;
using System.Net;
using Xamarin.Forms;

namespace BdP_MV
{
    
    public partial class App : Application
    {
        public static CookieContainer cookieContainer;

        public App()
        {
            App.cookieContainer = new CookieContainer();
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new Login();
            else
                MainPage = new NavigationPage(new Login());
        }
    }
}