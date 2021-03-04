using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Datenschutz : ContentPage

/* Nicht gemergte Änderung aus Projekt "BdP_MV.iOS"
Vor:
    {
        

        public Datenschutz()
Nach:
    {


        public Datenschutz()
*/
    {


        public Datenschutz()
        {
            InitializeComponent();
            try
            {
                Webview.Source = "https://meinbdp.de/display/MVHILFE/Datenschutz";
            }
            catch (Exception)
            {
            }
        }
        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await progress.ProgressTo(0.9, 900, Easing.SpringIn);
        }

        protected void OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            progress.IsVisible = true;
        }

        protected void OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            progress.IsVisible = false;
        }
    }
}