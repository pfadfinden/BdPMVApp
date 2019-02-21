using BdP_MV.Model;
using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BdP_MV.ViewModel
{
    public class LoginViewModel : BaseNavigationViewModel
    {
        public MainController mainc;
        public Connector_LoginDaten loginData;
        public LoginViewModel()
        {
            mainc = new MainController();
            loginData = new Connector_LoginDaten();


        }
        public async Task<string> CheckLogin(string username, string passwort)
        {
            loginData.Username = username;
            loginData.Password = passwort;
            IsBusy = true;
            string returnString = "";
            if (String.IsNullOrEmpty(loginData.Username))
            {
                returnString += "Bitte einen Benutzernamen eingeben";
            }
            else if (String.IsNullOrEmpty(loginData.Password))
            {
                returnString += "Bitte ein Passwort eingeben";
            }
            else
            {
                int answer = await Task.Run(async () => await mainc.mVConnector.LoginMV(loginData));
                if (answer==1)
                {
                    returnString += "Du bist schon vorher eingeloggt gewesen";
                }
                else if (answer ==2)
                {
                    returnString += "Die eingegebenen Logindaten sind falsch";

                }
                else if (answer == 3)
                {
                    returnString += "Es ist ein Fehler mit deiner Internetverbindung aufgetreten.";

                }
                else if (answer == 0)
                {
                    await mainc.groupControl.AlleGruppenAbrufen(0, "");
                    App.Current.Properties["Gruppen"] = mainc.groupControl.alleGruppen;

                }
                else
                {
                    returnString += "Irgendetwas seltsames ist aufgetreten";
                }
            }
            IsBusy = false;

            return returnString;
            


        }
       

    }
}
