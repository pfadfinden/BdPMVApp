using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BdP_MV.View
{
    public partial class EditMitglied : ContentPage
    {
       



        public EditMitglied()
        {
            InitializeComponent();

           

            BindingContext = this;
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            MessagingCenter.Send(this, "AddItem", Item);
            await Navigation.PopToRootAsync();
        }
    }
}
