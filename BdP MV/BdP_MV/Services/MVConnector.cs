using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using BdP_MV.Model;
using Newtonsoft.Json;

namespace BdP_MV.Services
{
    public class MVConnector
    {
        private bool isLoggedIn = false;
        private CookieContainer cookieContainer = new CookieContainer();
        private bool debug = false;

        public bool IsLoggedIn { get => isLoggedIn; }

        public void LoginMV(Connector_LoginDaten LoginDaten)
        {


            try
            {
                if (isLoggedIn)
                {
                    return;
                }
                HttpWebRequest request_first = (HttpWebRequest)HttpWebRequest.Create("https://mv.meinbdp.de/");
                request_first.CookieContainer = cookieContainer;

                HttpWebResponse response_first = (HttpWebResponse)request_first.GetResponse();
                int cookieCount = cookieContainer.Count;


                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/nami/auth/manual/sessionStartup");
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
                WebResponse response = (HttpWebResponse)request.GetResponse();
                string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();





                if (debug)
                {
                    Console.WriteLine(responseString);
                }

                HttpWebRequest request_nachricht = (HttpWebRequest)HttpWebRequest.Create("https://mv.meinbdp.de/ica/rest/dashboard/botschaft/current-message");
                request_nachricht.CookieContainer = cookieContainer;

                HttpWebResponse response_nachricht = (HttpWebResponse)request_nachricht.GetResponse();
                string response_nachricht_String = new StreamReader(response_nachricht.GetResponseStream()).ReadToEnd();
                if (debug)
                {
                    Console.WriteLine(response_nachricht_String);
                }
                Nachricht nachricht = JsonConvert.DeserializeObject<Nachricht>(response_nachricht_String);
                if (nachricht.success)
                {
                    isLoggedIn = true;

                }
                else
                {
                    cookieContainer = new CookieContainer();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

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

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/nami/gruppierungen/filtered-for-navigation/gruppierung/node/" + idname);
            request.Method = "GET";
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";

            WebResponse response = request.GetResponse();
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
        public List<Mitglied> Mitglieder(int idGruppe, bool nurAktiv)
        {
            HttpWebRequest request;
            if (nurAktiv)
            {
                request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/" + idGruppe + "/flist");
            }
            else
            {
                request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/" + idGruppe + "/flist");
            }
            request.Method = "GET";
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";

            WebResponse response = request.GetResponse();
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

        public MitgliedDetails MitgliedDetails(int idMitglied)
        {
            HttpWebRequest request;
            request = (HttpWebRequest)WebRequest.Create("https://mv.meinbdp.de/ica/rest/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/448/" + idMitglied);
            request.Method = "GET";
            request.CookieContainer = cookieContainer;
            request.ContentType = "application/x-www-form-urlencoded";
            WebResponse response = request.GetResponse();
            string responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            if (debug)
            {
                Console.WriteLine(responseString);
            }
            if (string.IsNullOrEmpty(responseString))
            {
                System.Threading.Thread.Sleep(10);
                response = request.GetResponse();
                responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();

            }
            MitgliedDetails mitgliedDetais = new MitgliedDetails();
            if (string.IsNullOrEmpty(responseString))
            {
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine("!!!!!!!!!!!!!!!!!!!!!!!!");
                Console.WriteLine("Abbruch auch nach Wait");
            }
            else

            {

                RootObjectMitgliedDetails listeAllerMitglieder = JsonConvert.DeserializeObject<RootObjectMitgliedDetails>(responseString);
                mitgliedDetais = listeAllerMitglieder.data;
            }
            return mitgliedDetais;
        }

    }
}
