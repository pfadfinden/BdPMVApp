using BdP_MV.Model;
using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.ViewModel
{
    public class LoginViewModel : BaseNavigationViewModel
    {
        MainController mainc;
        public Connector_LoginDaten loginData;
        public LoginViewModel()
        {
            mainc = new MainController();
            loginData = new Connector_LoginDaten();


        }
        public async Task<string> CheckLogin()
        {
            IsBusy = true;
            string returnString = "";

            IsBusy = false;
            return returnString;

        }

    }
}
