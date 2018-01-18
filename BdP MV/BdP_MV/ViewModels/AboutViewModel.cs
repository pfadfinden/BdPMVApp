using MvvmHelpers;
using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace BdP_MV.ViewModel
{
    public class AboutViewModel : BaseViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://www.pfadfinden.de")));
        }

        public ICommand OpenWebCommand { get; }
    }
}