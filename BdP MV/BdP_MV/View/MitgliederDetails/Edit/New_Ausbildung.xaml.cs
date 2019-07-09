using BdP_MV.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MitgliederDetails.Edit
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class New_Ausbildung : ContentPage
	{
		public New_Ausbildung ()
		{
			InitializeComponent ();
		}
        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
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
                btn_save.IsEnabled = true;



            }
            catch (NoRightsException ex)
            {
                await DisplayAlert("Fehler", "Für diesen Vorgang hast du keine Rechte.", "OK");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);

            }
            catch (NotAllRequestedFieldsFilledException ex)
            {
                await DisplayAlert("Fehler", ex.Message, "OK");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                btn_save.IsEnabled = true;


            }
            catch (NullReferenceException ex)
            {
                await DisplayAlert("Fehler", ex.Message, "OK");
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
                btn_save.IsEnabled = true;


            }

        }

    }
}