using BdP_MV.ViewModel;
using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BdP_MV.View.MitgliederDetails
{
    public partial class NewMitglied : ContentPage
    {

        NewMitgliedViewModel viewModel;
        int idGruppe;



        public NewMitglied(int idgrp)
        {
            //   InitializeComponent();
            // this.viewModel = new NewMitgliedViewModel(new Services.MainController());
            viewModel = new NewMitgliedViewModel();

            idGruppe = idgrp;
            InitializeComponent();
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
        }
    }
}
