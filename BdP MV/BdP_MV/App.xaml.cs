using BdP_MV.View.LoginForms;
using System.Net.Http;
using Xamarin.Forms;

namespace BdP_MV
{

    public partial class App : Application
    {
        public static HttpClient client;

        public App()
        {
            App.client = new HttpClient();
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
            {
                MainPage = new Login();
            }
            else
            {
                MainPage = new NavigationPage(new Login());
            }
        }
    }
}