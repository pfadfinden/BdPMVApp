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
            String rueckmeldung;
            if (String.IsNullOrEmpty(pwData.emailTo)||String.IsNullOrEmpty(pwData.MitgliedsNummer))
            {
                rueckmeldung = "Bitte fülle alle Felder aus";
            }

            else
            {
                int result = await mainC.mVConnector.RequestNewPassword(pwData);
                if (result ==2)
                {
                    rueckmeldung = "Das Passwort konnte nicht zurück gesetzt werden, bitte gib korrekte Daten ein.";
                }
                else if (result ==3)
                {
                    rueckmeldung = " Fehler bei der Internetanbindung";
                }
                else
                {
                    rueckmeldung = "";
                }
                
            }

            IsBusy = false;
            return rueckmeldung;
        }
    }
}
