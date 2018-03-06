using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BdP_MV.Model;
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
                HttpWebRequest request_first;
                if (qa)
                {
                     request_first= (HttpWebRequest)HttpWebRequest.Create("https://qa.mv.meinbdp.de/");
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
            HttpWebRequest request;
            if (qa)
            {
                request = (HttpWebRequest)WebRequest.Create("https://qa.mv.meinbdp.de/ica/rest/nami/gruppierungen/filtered-for-navigation/gruppierung/node/" + idname);
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/nami/gruppierungen/filtered-for-navigation/gruppierung/node/" + idname);
            }
           
            request.Method = "GET";
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Console.WriteLine("abc");
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (debug)
            {
                Console.WriteLine(responseString);
            }
            List<Gruppe> gruppen = new List<Gruppe>();

            GroupList listeAllerUntergruppen =  JsonConvert.DeserializeObject<GroupList>(responseString);

            gruppen = listeAllerUntergruppen.data;

            return gruppen;


        }
        //public async Task<List<Mitglied>> Mitglieder(int idGruppe, bool nurAktiv)
        public async Task<List<Mitglied>> Mitglieder(int idGruppe, bool nurAktiv)
        {
            HttpWebRequest request;
            if (qa)
            {
                request = (HttpWebRequest)WebRequest.Create("https://qa.mv.meinbdp.de/ica/rest/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/" + idGruppe + "/flist");
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/" + idGruppe + "/flist");
            }
         
            request.Method = "GET";
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            if (debug)
            {
                Console.WriteLine(responseString);
            }
            List<Mitglied> mitglieder = new List<Mitglied>();
            

            MitgliederListe listeAllerMitglieder = JsonConvert.DeserializeObject<MitgliederListe>(responseString);
            mitglieder = listeAllerMitglieder.data;

            return mitglieder;
        }
        //public List<Taetigkeit> Taetigkeiten(int idMitglied)
        //{
        //    HttpWebRequest request;
        //    request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/nami/zugeordnete-taetigkeiten/filtered-for-navigation/gruppierung-mitglied/mitglied/" + idMitglied + "/flist");
        //    request.Method = "GET";
        //    request.CookieContainer = cookieContainer;
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    WebResponse response = request.GetResponse();
        //    string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
        //    if (debug)
        //    {
        //        Console.WriteLine(responseString);
        //    }
        //    List<Taetigkeit> taetigkeiten = new List<Taetigkeit>();
        //    AlleTaetigkeiten listeAllerMitglieder = new JsonConvert.DeserializeObject<AlleTaetigkeiten>(responseString);
        //    taetigkeiten = listeAllerMitglieder.data;

        //    return taetigkeiten;
        //}

        public async Task<MitgliedDetails> MitgliedDetails(int idMitglied)
        {
            HttpWebRequest request;
            if (qa)
            {
                request = (HttpWebRequest)WebRequest.Create("https://qa.mv.meinbdp.de/ica/rest/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/448/" + idMitglied);
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/448/" + idMitglied);
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
           
            MitgliedDetails mitgliedDetais = new MitgliedDetails();
            RootObjectMitgliedDetails listeAllerMitglieder = JsonConvert.DeserializeObject<RootObjectMitgliedDetails>(responseString);
            mitgliedDetais = listeAllerMitglieder.data;
            
            return mitgliedDetais;
        }

    }
}
