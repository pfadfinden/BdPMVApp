﻿using BdP_MV.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MitgliederDetails
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Ausbildung_Liste : ContentPage
    {
        private ItemDetailViewModel viewModel;

        public Ausbildung_Liste()
        {
            InitializeComponent();

        }
        public Ausbildung_Liste(ItemDetailViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = this.viewModel = viewModel;

        }

        async void Handle_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item == null)
                return;

            await DisplayAlert("Item Tapped", "An item was tapped.", "OK");

            //Deselect Item
            ((ListView)sender).SelectedItem = null;
        }
    }
}
