using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BdP_MV.Exceptions;
using BdP_MV.Model;
using BdP_MV.Model.Metamodel;
using BdP_MV.Model.Mitglied;
using Newtonsoft.Json;


namespace BdP_MV.Services
{
    public class MVConnector
    {
        private bool isLoggedIn = false;
        private CookieContainer cookieContainer = new CookieContainer();
        private bool debug = false;
        Boolean qa = true;

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
                App.Current.Properties["cookieContainer"] = cookieContainer;
                HttpWebRequest request_nachricht;
                if (qa)
                {
                    request_nachricht = (HttpWebRequest)HttpWebRequest.Create("https://qa.mv.meinbdp.de/ica/rest/dashboard/botschaft/current-message");
                }
                else
                {
                    request_nachricht = (HttpWebRequest)HttpWebRequest.Create("https://mv.meinbdp.de/ica/rest/dashboard/botschaft/current-message");
                }

                request_nachricht.CookieContainer = (CookieContainer)App.Current.Properties["cookieContainer"];

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
            String anfrage = "nami/search-multi/result-list?searchedValues=" + suchanfrage +"&page=1&start=0&limit=9999999";
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
            RootObject_SGB8 rootFZ = JsonConvert.DeserializeObject<RootObject_SGB8>(responseString);
            List<SGB8> fuehrungszeugnisse = rootFZ.data;
            return fuehrungszeugnisse;
        }
        
        public async Task<List<Taetigkeit>> Taetigkeiten (int idMitglied)
        {
            string anfrage = "/nami/zugeordnete-taetigkeiten/filtered-for-navigation/gruppierung-mitglied/mitglied/" + idMitglied + "/flist";
            string responseString = await GetApiResultStringAsync(anfrage);
            RootObject_Taetigkeit rootObjectTaetigkeiten = JsonConvert.DeserializeObject<RootObject_Taetigkeit>(responseString);
            List<Taetigkeit> taetigkeiten = rootObjectTaetigkeiten.data;
            return taetigkeiten;
        }
        public async Task<Meta_Data> MetaData(int idGruppe)
        {
            string anfrage = "nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/" + idGruppe + "/META";
            string responseString = await GetApiResultStringAsync(anfrage);
            RootObject_Meta_Data rootObjectMetadata = JsonConvert.DeserializeObject<RootObject_Meta_Data>(responseString);

            Meta_Data metaData = rootObjectMetadata.data;
            return metaData;
        }
        public async Task<List<Ausbildung>> Ausbildung(int idMitglied)
        {
            string anfrage = "nami/mitglied-ausbildung/filtered-for-navigation/mitglied/mitglied/" + idMitglied + "/flist";
            string responseString = await GetApiResultStringAsync(anfrage);
            RootObject_Ausbildung rootObject= JsonConvert.DeserializeObject<RootObject_Ausbildung>(responseString);
            List<Ausbildung> ausbildungen = rootObject.data;
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
        public async Task<List<Report_Data>> ReportData(int idGruppe)
        {

            String anfrage = "nami/grp-reports/filtered-for-grpadmin/gruppierung/crtGruppierung/"+idGruppe+"/flist";
            String responseString = await GetApiResultStringAsync(anfrage);

            List <Report_Data> reportList = new List<Report_Data>();
            Report_RootObject root_Reports = JsonConvert.DeserializeObject<Report_RootObject>(responseString);
            if (root_Reports.success == false)
            {
                if (root_Reports.responseType.Equals("ERROR") && root_Reports.message.Equals("Session expired"))
                {
                    throw new NewLoginException("Bitte neu einloggen.");
                }
                if (root_Reports.responseType.Equals("EXCEPTION") && root_Reports.message.Equals("Benutzer darf sich keine GrpReports ansehen"))
                {
                    throw new NoRightsException("Versucht Kontext aufzurufen, für das der Nutzer keine Rechte hat.");
                }
            }
            reportList = root_Reports.data;

            return reportList;
        }
        private async Task<String> GetApiResultStringAsync(string anfrageURL)
        {
            cookieContainer = (CookieContainer)App.Current.Properties["cookieContainer"];

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
            cookieContainer = (CookieContainer)App.Current.Properties["cookieContainer"];

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
        private async Task<String> PostApiData(string anfrageURL, string postData)
        {
            cookieContainer = (CookieContainer)App.Current.Properties["cookieContainer"];

            HttpWebRequest request;
            if (qa)
            {
                request = (HttpWebRequest)WebRequest.Create("https://qa.mv.meinbdp.de/ica/rest/" + anfrageURL);
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/" + anfrageURL);
            }
            request.Method = "POST";
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";
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
            return responseString;

        }
        public async Task<String> PostNewMitglied(int idGruppe, string JSOPM)
        {

            String anfrage = "nami/grp-reports/filtered-for-grpadmin/gruppierung/crtGruppierung/" + idGruppe + "/flist";
            String responseString = await GetApiResultStringAsync(anfrage);

            var response = JsonConvert.DeserializeObject<Report_RootObject>(responseString);
            if (response.success == false)
            {
                if (response.responseType.Equals("ERROR") && response.message.Equals("Session expired"))
                {
                    throw new NewLoginException("Bitte neu einloggen.");
                }
                if (response.responseType.Equals("EXCEPTION") && response.message.Equals("Benutzer darf sich keine GrpReports ansehen"))
                {
                    throw new NoRightsException("Versucht Kontext aufzurufen, für das der Nutzer keine Rechte hat.");
                }
            }
     

            return "123";
        }


    }
}
