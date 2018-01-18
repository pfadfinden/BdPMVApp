using BdP_MV.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        protected LoginViewModel ViewModel => BindingContext as LoginViewModel;

        public Login ()
		{
			InitializeComponent ();
            

        }
        async void OnSignUpButtonClicked(object sender, EventArgs e)
        {
         //   await Navigation.PushAsync(new SignUpPage());
        }

        async void OnLoginButtonClicked(object sender, EventArgs e)
        {
            ViewModel.loginData.Username = usernameEntry.Text;
            ViewModel.loginData.Password = passwordEntry.Text;
            Boolean isValid = true;          
            if (isValid)
            {
                Navigation.InsertPageBefore(new ItemsPage(), this);
                await Navigation.PopAsync();
            }
            else
            {
                messageLabel.Text = "Login failed";
                passwordEntry.Text = string.Empty;
            }
        }

     
    }
}