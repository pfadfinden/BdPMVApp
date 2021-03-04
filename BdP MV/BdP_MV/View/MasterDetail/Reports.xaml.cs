using BdP_MV.Exceptions;
using BdP_MV.Model;
using BdP_MV.ViewModel;
using System;
using System.Net;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Reports : ContentPage

    {
        ReportViewModel viewModel;
        public Reports()
        {
            InitializeComponent();
        }
        public async void thePickerSelectedIndexChanged(object sender, EventArgs e)
        {

            try
            {
                //await DisplayAlert("Ausgewählte Gruppe", viewModel.aktGruppe.id.ToString(), "OK");//Method call every time when picker selection changed
                IsBusy = true;
                viewModel.aktGruppe = (Gruppe)testpicker.SelectedItem;
                await viewModel.GetReportsByGroup(viewModel.aktGruppe.id);

                ReportView.ItemsSource = viewModel.reportdata;


                IsBusy = false;
            }

            catch (NewLoginException ex)
            {
                await DisplayAlert("Fehler", "Deine Sitzung ist abgelaufen. Bitte logge dich neu in die App ein.", "OK");
                Navigation.InsertPageBefore(new LoginForms.Login(), this);
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                await Navigation.PopAsync();

            }
            catch (WebException ex)
            {
                await DisplayAlert("Fehler", "Fehler beim Herstellen der Internetverbindung", "OK");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);


            }
            catch (NoRightsException ex)
            {
                await DisplayAlert("Fehler", "Für diesen Vorgang hast du keine Rechte.", "OK");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            IsBusy = false;


        }
        async void ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}