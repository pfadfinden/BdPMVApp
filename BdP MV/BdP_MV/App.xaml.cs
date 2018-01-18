using BdP_MV.View;
using System;

using Xamarin.Forms;

namespace BdP_MV
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            if (Device.RuntimePlatform == Device.iOS)
                MainPage = new Login();
            else
                MainPage = new NavigationPage(new Login());
        }
    }
}