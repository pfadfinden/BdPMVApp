using BdP_MV.Model;
using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.ViewModel
{
    public class ForgotPWViewModel : BaseNavigationViewModel
    {
        public MainController mainC;
        private ResetPassword pwData;
        public ForgotPWViewModel(MainController mainCo)
        {
            mainC = mainCo;


        }
        public async Task<String> resetPW(string username, string gebDatum, string email)
        {
            IsBusy = true;
            pwData.emailTo = email;
            pwData.geburtsDatum = gebDatum;
            pwData.MitgliedsNummer = username;

            IsBusy = false;
            return "";
        }
    }
}
