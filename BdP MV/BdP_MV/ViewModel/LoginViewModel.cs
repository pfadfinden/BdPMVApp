﻿using BdP_MV.Model;
using BdP_MV.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

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
        public async Task LoadGroups()
        {
            IsBusy = true;
            
            await mainc.groupControl.AlleGruppenAbrufen(0, "");
               
    
            
            
            IsBusy = false;
            return;
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
                loginData.Username = loginData.Username.Trim();
                loginData.Password = loginData.Password.Trim();
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
