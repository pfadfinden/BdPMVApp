using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BdP_MV.Exceptions;
using BdP_MV.Model;
using BdP_MV.Model.Mitglied;
using Newtonsoft.Json;

namespace BdP_MV.Services
{
    public class MVConnector
    {
        private bool isLoggedIn = false;
        private CookieContainer cookieContainer = new CookieContainer();
        private bool debug = false;
        Boolean qa = false;

        public bool IsLoggedIn { get => isLoggedIn; }

        public async Task<int> LoginMV(Connector_LoginDaten LoginDaten)
        {


            try
            {
                if (isLoggedIn)
                {
                    return 1; //Ist Bereits eingeloggt
                }
                //HttpWebRequest request_first;
                //if (qa)
                //{
                //     request_first= (HttpWebRequest)HttpWebRequest.Create("https://qa.mv.meinbdp.de/");
                //}
                //else
                //{
                //    request_first = (HttpWebRequest)HttpWebRequest.Create("https://mv.meinbdp.de/");
                //}
                //request_first.CookieContainer = cookieContainer;

                //HttpWebResponse response_first = (HttpWebResponse)await request_first.GetResponseAsync();
                //int cookieCount = cookieContainer.Count;
                HttpWebRequest request;
                if (qa)
                {
                    request = (HttpWebRequest)WebRequest.Create("https://qa.mv.meinbdp.de/ica/rest/nami/auth/manual/sessionStartup");
                }
                else
                {
                    request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/nami/auth/manual/sessionStartup");
                }
                request.Method = "POST";
                request.CookieContainer = cookieContainer;
                request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                string postData = "username=" + LoginDaten.Username + "&password=" + LoginDaten.Password + "&redirectTo=app.jsp&Login=Anmelden";
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                byte[] bytes = iso.GetBytes(postData);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                WebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (debug)
                {
                    Console.WriteLine(responseString);
                }
                HttpWebRequest request_nachricht;
                if (qa)
                {
                    request_nachricht = (HttpWebRequest)HttpWebRequest.Create("https://qa.mv.meinbdp.de/ica/rest/dashboard/botschaft/current-message");
                }
                else
                {
                    request_nachricht = (HttpWebRequest)HttpWebRequest.Create("https://mv.meinbdp.de/ica/rest/dashboard/botschaft/current-message");
                }
                
                request_nachricht.CookieContainer = cookieContainer;

                HttpWebResponse response_nachricht = (HttpWebResponse)await request_nachricht.GetResponseAsync();
                string response_nachricht_String = new StreamReader(response_nachricht.GetResponseStream()).ReadToEnd();
                if (debug)
                {
                    Console.WriteLine(response_nachricht_String);
                }
                Nachricht nachricht = JsonConvert.DeserializeObject<Nachricht>(response_nachricht_String);
                int cookieCount = cookieContainer.Count;
             
             
                
               
                if (nachricht.success)
                {
                    isLoggedIn = true;
                    return 0;

                }
                else
                {
                    cookieContainer = new CookieContainer();
                    return 2; //Falsche LoginDaten
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 3;

            }

        }
        public async Task<int> RequestNewPassword(ResetPassword resetPassword)
        {
            try
            {
                if (isLoggedIn)
                {
                    return 1; //Ist Bereits eingeloggt
                }
                HttpWebRequest request_first;
                if (qa)
                {
                    request_first = (HttpWebRequest)HttpWebRequest.Create("https://qa.mv.meinbdp.de/");
                }
                else
                {
                    request_first = (HttpWebRequest)HttpWebRequest.Create("https://mv.meinbdp.de/");
                }
                request_first.CookieContainer = cookieContainer;

                HttpWebResponse response_first = (HttpWebResponse)await request_first.GetResponseAsync();
                int cookieCount = cookieContainer.Count;
                HttpWebRequest request;
                if (qa)
                {
                    request = (HttpWebRequest)WebRequest.Create("https://qa.mv.meinbdp.de/ica/rest/nami/auth/resetPassword");
                }
                else
                {
                    request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/nami/auth/resetPassword");
                }
                request.Method = "POST";
                request.CookieContainer = cookieContainer;
                request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
                string postData = "mitgliedsNummer=" + resetPassword.MitgliedsNummer + "&geburtsDatum=" + resetPassword.geburtsDatum + "&emailTo=" + resetPassword.emailTo+"&Login=Neues+Passwort+zusenden";
                Encoding iso = Encoding.GetEncoding("ISO-8859-1");
                byte[] bytes = iso.GetBytes(postData);
                request.ContentLength = bytes.Length;
                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
                WebResponse response = (HttpWebResponse)await request.GetResponseAsync();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
                if (responseString.Contains("Ein neues Passwort wurde an Ihre hinterlegte E-Mail-Adresse versendet"))
                    return 0;
                else if (responseString.Contains("Ihre Angaben sind nicht korrekt"))
                    return 2;
                else return 3;
               

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 3;

            }
        }
        public async Task<List<SelectableItem>> GetItems(String anfrage)
        {
            string responseString = await GetApiResultStringAsync(anfrage);
            List<SelectableItem> items = new List<SelectableItem>();
            RootObjectItem rootItem = JsonConvert.DeserializeObject<RootObjectItem>(responseString);
            items = rootItem.data;

            return items;
        }
        public List<Gruppe> GetGroups(int id)

        {
            string idname;
            if (id == 0)
            {
                if (debug)
                {
                    Console.WriteLine("RootGruppe");
                }

                idname = "root";
            }
            else
            {
                if (debug)
                {
                    Console.WriteLine("Gruppenid:" + id);
                }
                idname = id.ToString();
            }
            String anfrage = "nami/gruppierungen/filtered-for-navigation/gruppierung/node/" + idname;
            string responseString = GetApiResultString(anfrage);

            if (debug)
            {
                Console.WriteLine(responseString);
            }
            List<Gruppe> gruppen = new List<Gruppe>();

            GroupList listeAllerUntergruppen =  JsonConvert.DeserializeObject<GroupList>(responseString);
            if (listeAllerUntergruppen.success==false)
            {
                if (listeAllerUntergruppen.responseType.Equals("ERROR")&& listeAllerUntergruppen.message.Equals("Session expired"))
                {
                    throw new NewLoginException("Bitte neu einloggen.");
                }
            }

            gruppen = listeAllerUntergruppen.data;

            return gruppen;


        }
        //public async Task<List<Mitglied>> Mitglieder(int idGruppe, bool nurAktiv)
        public async Task<List<Mitglied>> Mitglieder(int idGruppe, bool nurAktiv)
        {
            String anfrage = "nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/" + idGruppe + "/flist";
            string responseString = await GetApiResultStringAsync(anfrage);
            List<Mitglied> mitglieder = new List<Mitglied>();
            MitgliederListe listeAllerMitglieder = JsonConvert.DeserializeObject<MitgliederListe>(responseString);
            mitglieder = listeAllerMitglieder.data;

            return mitglieder;
        }

        public async Task<List<Mitglied>> Mitglieder(string suchanfrage)
        {
            String anfrage = "nami/search-multi/result-list?searchedValues=" + suchanfrage;
            string responseString = await GetApiResultStringAsync(anfrage);
            List<Mitglied> mitglieder = new List<Mitglied>();
            MitgliederListe listeAllerMitglieder = JsonConvert.DeserializeObject<MitgliederListe>(responseString);
            mitglieder = listeAllerMitglieder.data;

            return mitglieder;
        }
        public async Task<List<SGB8>> SGB8(int idMitglied)
        {
            string anfrage = "/nami/mitglied-sgb-acht/filtered-for-navigation/empfaenger/empfaenger/" + idMitglied + "/flist";
            string responseString = await GetApiResultStringAsync(anfrage);
            List<SGB8> fuehrungszeugnisse = new List<SGB8>();
            RootObject_SGB8 rootFZ = JsonConvert.DeserializeObject<RootObject_SGB8>(responseString);
            fuehrungszeugnisse = rootFZ.data;
            return fuehrungszeugnisse;
        }
        
        public async Task<List<Taetigkeit>> Taetigkeiten (int idMitglied)
        {
            string anfrage = "/nami/zugeordnete-taetigkeiten/filtered-for-navigation/gruppierung-mitglied/mitglied/" + idMitglied + "/flist";
            string responseString = await GetApiResultStringAsync(anfrage);
            List<Taetigkeit> taetigkeiten = new List<Taetigkeit>();
            RootObject_Taetigkeit rootObjectTaetigkeiten = JsonConvert.DeserializeObject<RootObject_Taetigkeit>(responseString);
            taetigkeiten = rootObjectTaetigkeiten.data;
            return taetigkeiten;
        }
        public async Task<List<Ausbildung>> Ausbildung(int idMitglied)
        {
            string anfrage = "nami/mitglied-ausbildung/filtered-for-navigation/mitglied/mitglied/" + idMitglied + "/flist";
            string responseString = await GetApiResultStringAsync(anfrage);
            List<Ausbildung> ausbildungen = new List<Ausbildung>();
            RootObject_Ausbildung rootObject= JsonConvert.DeserializeObject<RootObject_Ausbildung>(responseString);
            ausbildungen = rootObject.data;
            return ausbildungen;
        }

        public async Task<MitgliedDetails> MitgliedDetails(int idMitglied, int idGruppe)
        {
            
            String anfrage ="nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/"+idGruppe+"/"+idMitglied;
            String responseString = await GetApiResultStringAsync(anfrage);

            MitgliedDetails mitgliedDetais = new MitgliedDetails();
            RootObjectMitgliedDetails listeAllerMitglieder = JsonConvert.DeserializeObject<RootObjectMitgliedDetails>(responseString);
            if (listeAllerMitglieder.success == false)
            {
                if (listeAllerMitglieder.responseType.Equals("ERROR") && listeAllerMitglieder.message.Equals("Session expired"))
                {
                    throw new NewLoginException("Bitte neu einloggen.");
                }
                if (listeAllerMitglieder.responseType.Equals("EXCEPTION") && listeAllerMitglieder.message.Equals("Access denied - no right for requested operation"))
                {
                    throw new NoRightsException("Versucht Kontext aufzurufen, für das der Nutzer keine Rechte hat.");
                }
            }
            mitgliedDetais = listeAllerMitglieder.data;
            
            return mitgliedDetais;
        }
        public async Task<Ausbildung_Details> AusbildungDetails(int idAusbildung, int idMitglied)
        {

            String anfrage = "nami/mitglied-ausbildung/filtered-for-navigation/mitglied/mitglied/" + idMitglied + "/" + idAusbildung;
            String responseString = await GetApiResultStringAsync(anfrage);

            Ausbildung_Details ausbildungDetails = new Ausbildung_Details();
            RootObject_Ausbildung_Details rootAusbildung_Details = JsonConvert.DeserializeObject<RootObject_Ausbildung_Details>(responseString);
            if (rootAusbildung_Details.success == false)
            {
                if (rootAusbildung_Details.responseType.Equals("ERROR") && rootAusbildung_Details.message.Equals("Session expired"))
                {
                    throw new NewLoginException("Bitte neu einloggen.");
                }
                if (rootAusbildung_Details.responseType.Equals("EXCEPTION") && rootAusbildung_Details.message.Equals("Access denied - no right for requested operation"))
                {
                    throw new NoRightsException("Versucht Kontext aufzurufen, für das der Nutzer keine Rechte hat.");
                }
            }
            ausbildungDetails = rootAusbildung_Details.data;

            return ausbildungDetails;
        }
        private async Task<String> GetApiResultStringAsync(string anfrageURL)
        {
            HttpWebRequest request;
            if (qa)
            {
                request = (HttpWebRequest)WebRequest.Create("https://qa.mv.meinbdp.de/ica/rest/"+anfrageURL);
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/" + anfrageURL);
            }
            request.Method = "GET";
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            if (debug)
            {
                Console.WriteLine(responseString);
            }
            return responseString;


        }
        private String GetApiResultString(string anfrageURL)
        {
            HttpWebRequest request;
            if (qa)
            {
                request = (HttpWebRequest)WebRequest.Create("https://qa.mv.meinbdp.de/ica/rest/" + anfrageURL);
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/" + anfrageURL);
            }
            request.Method = "GET";
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = (HttpWebResponse)request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            if (debug)
            {
                Console.WriteLine(responseString);
            }
            return responseString;


        }

    }
}
