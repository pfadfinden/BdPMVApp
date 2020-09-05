using BdP_MV.Exceptions;
using BdP_MV.Model;
using BdP_MV.Model.Metamodel;
using BdP_MV.Model.Mitglied;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.Services
{
    public class MVConnector
    {
        private bool isLoggedIn = false;
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
                //request_f irst.CookieContainer = cookieContainer;

                //HttpWebResponse response_first = (HttpWebResponse)await request_first.GetResponseAsync();
                //int cookieCount = cookieContainer.Count;
                String url;
                if(qa)
                {
                     url = "https://qa.mv.meinbdp.de/ica/rest/nami/auth/manual/sessionStartup";
                }
                else
                {
                     url = "https://mv.meinbdp.de/ica/rest/nami/auth/manual/sessionStartup";
                }
                App.client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                FormUrlEncodedContent formContent = new FormUrlEncodedContent(new[]
                {
                     new KeyValuePair<string, string>("username", LoginDaten.Username),
                     new KeyValuePair<string, string>("password", LoginDaten.Password ),
                     new KeyValuePair<string, string>("redirectTo", "app.jsp"),
                     new KeyValuePair<string, string>("Login", "Anmelden")
                 });
                HttpResponseMessage response = await App.client.PostAsync(url, formContent);

                string responseString = await response.Content.ReadAsStringAsync();
                if (debug)
                {
                    Console.WriteLine(responseString);
                }
                
                if (qa)
                {
                    url="https://qa.mv.meinbdp.de/ica/rest/dashboard/botschaft/current-message";
                }
                else
                {
                    url = "https://mv.meinbdp.de/ica/rest/dashboard/botschaft/current-message";
                }

                HttpResponseMessage response_nachricht = await App.client.GetAsync(url);
                
                string response_nachricht_String = await response_nachricht.Content.ReadAsStringAsync();
                if (debug)
                {
                    Console.WriteLine(response_nachricht_String);
                }
                Nachricht nachricht = JsonConvert.DeserializeObject<Nachricht>(response_nachricht_String);

                if (nachricht.success)
                {
                    isLoggedIn = true;
                    App.Current.Properties["news"] = nachricht.data;
                    return 0;

                }
                else
                {
                    return 2; //Falsche LoginDaten
                    isLoggedIn = true;
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
                String url;

                if (qa)
                {
                    url = "https://qa.mv.meinbdp.de/";
                }
                else
                {
                    url = "https://mv.meinbdp.de/";
                }
                HttpResponseMessage response_first = await App.client.GetAsync(url);
                
                if (qa)
                {
                    url ="https://qa.mv.meinbdp.de/ica/rest/nami/auth/resetPassword";
                }
                else
                {
                    url = "https://mv.meinbdp.de/ica/rest/nami/auth/resetPassword";
                }
                
                string postData = "mitgliedsNummer=" + resetPassword.MitgliedsNummer + "&geburtsDatum=" + resetPassword.geburtsDatum + "&emailTo=" + resetPassword.emailTo + "&Login=Neues+Passwort+zusenden";


                HttpResponseMessage response = await App.client.PostAsync(url, new StringContent(postData));
                string responseString = await response.Content.ReadAsStringAsync();
                if (responseString.Contains("Ein neues Passwort wurde an Ihre hinterlegte E-Mail-Adresse versendet"))
                {
                    return 0;
                }
                else if (responseString.Contains("Ihre Angaben sind nicht korrekt"))
                {
                    return 2;
                }
                else
                {
                    return 3;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return 3;

            }
        }
        public async Task<List<SelectableItem>> GetItems(String anfrage)
        {
            string responseString = await GetApiResultStringAsync(anfrage).ConfigureAwait(false);
            List<SelectableItem> items = new List<SelectableItem>();
            RootObjectItem rootItem = JsonConvert.DeserializeObject<RootObjectItem>(responseString);
            items = rootItem.data;

            return items;
        }
        public async Task<List<Gruppe>> GetGroups(int id)

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
            String anfrage = "api/1/2/service/nami/gruppierungen/filtered-for-navigation/gruppierung/node/" + idname;
            string responseString = await GetApiResultStringAsync(anfrage);

            if (debug)
            {
                Console.WriteLine(responseString);
            }
            List<Gruppe> gruppen = new List<Gruppe>();
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            GroupList listeAllerUntergruppen = (GroupList)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(GroupList));
            if (listeAllerUntergruppen.success == false)
            {
                if (listeAllerUntergruppen.responseType.Equals("ERROR") && listeAllerUntergruppen.message.Equals("Session expired"))
                {
                    throw new NewLoginException("Bitte neu einloggen.");
                }
            }

            gruppen = listeAllerUntergruppen.data;

            return gruppen;


        }
        public async Task<List<Mitglied>> Mitglieder(int idGruppe, bool nurAktiv)
        {
            String anfrage = "api/1/2/service/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/" + idGruppe + "/flist";
            string responseString = await GetApiResultStringAsync(anfrage).ConfigureAwait(false);
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            MitgliederListe listeAllerMitglieder = (MitgliederListe)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(MitgliederListe));
            List<Mitglied> mitglieder = new List<Mitglied>();
            mitglieder = listeAllerMitglieder.data;

            return mitglieder;
        }
        public async Task<List<Mitglied>> Mitglieder(string suchanfrage)
        {
            String anfrage = "nami/search-multi/result-list?searchedValues=" + suchanfrage + "&page=1&start=0&limit=9999999";
            string responseString = await GetApiResultStringAsync(anfrage).ConfigureAwait(false);
            List<Mitglied> mitglieder = new List<Mitglied>();
            MitgliederListe listeAllerMitglieder = JsonConvert.DeserializeObject<MitgliederListe>(responseString);

            mitglieder = listeAllerMitglieder.data;

            return mitglieder;
        }
        public async Task<List<SGB8>> SGB8(int idMitglied)
        {
            string anfrage = "api/1/2/service/nami/mitglied-sgb-acht/filtered-for-navigation/empfaenger/empfaenger/" + idMitglied + "/flist";
            string responseString = await GetApiResultStringAsync(anfrage).ConfigureAwait(false);
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            RootObject_SGB8 rootFZ = (RootObject_SGB8)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(RootObject_SGB8));
            List<SGB8> fuehrungszeugnisse = rootFZ.data;
            return fuehrungszeugnisse;
        }
        public async Task<List<Taetigkeit>> Taetigkeiten(int idMitglied)
        {
            string anfrage = "api/1/2/service/nami/zugeordnete-taetigkeiten/filtered-for-navigation/gruppierung-mitglied/mitglied/" + idMitglied + "/flist";
            string responseString = await GetApiResultStringAsync(anfrage).ConfigureAwait(false);
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            RootObject_Taetigkeit rootObjectTaetigkeiten = (RootObject_Taetigkeit)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(RootObject_Taetigkeit));

            List<Taetigkeit> taetigkeiten = rootObjectTaetigkeiten.data;
            return taetigkeiten;
        }
        public async Task<Meta_Data> MetaDataGruppierung(int idGruppe)
        {
            string anfrage = "api/1/2/service/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/" + idGruppe + "/META";
            string responseString = await GetApiResultStringAsync(anfrage).ConfigureAwait(false);
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            RootObject_Meta_Data rootObjectMetadata = (RootObject_Meta_Data)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(RootObject_Meta_Data));



            Meta_Data metaData = rootObjectMetadata.data;
            return metaData;
        }
        public async Task<Meta_Data> MetaDataAusbildung(int idMitglied)
        {
            string anfrage = "api/1/2/service/nami/mitglied-ausbildung/filtered-for-navigation/mitglied/mitglied/" + idMitglied + "/META";
            string responseString = await GetApiResultStringAsync(anfrage).ConfigureAwait(false);
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            RootObject_Meta_Data rootObjectMetadata = (RootObject_Meta_Data)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(RootObject_Meta_Data));



            Meta_Data metaData = rootObjectMetadata.data;
            return metaData;
        }
        public async Task<List<Ausbildung>> Ausbildung(int idMitglied)
        {
            string anfrage = "api/1/2/service/nami/mitglied-ausbildung/filtered-for-navigation/mitglied/mitglied/" + idMitglied + "/flist";
            string responseString = await GetApiResultStringAsync(anfrage).ConfigureAwait(false);
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            RootObject_Ausbildung rootObject = (RootObject_Ausbildung)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(RootObject_Ausbildung));
            List<Ausbildung> ausbildungen = rootObject.data;
            return ausbildungen;
        }
        public async Task<MitgliedDetails> MitgliedDetails(int idMitglied, int idGruppe)
        {

            String anfrage = "api/1/2/service/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/" + idGruppe + "/" + idMitglied;
            String responseString = await GetApiResultStringAsync(anfrage).ConfigureAwait(false);

            MitgliedDetails mitgliedDetais = new MitgliedDetails();
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            RootObjectMitgliedDetails rootMitgliedDetails = (RootObjectMitgliedDetails)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(RootObjectMitgliedDetails));
            if (rootMitgliedDetails.success == false)
            {
                if (rootMitgliedDetails.responseType.Equals("ERROR") && rootMitgliedDetails.message.Equals("Session expired"))
                {
                    throw new NewLoginException("Bitte neu einloggen.");
                }
                if (rootMitgliedDetails.responseType.Equals("EXCEPTION") && rootMitgliedDetails.message.Equals("Access denied - no right for requested operation"))
                {
                    throw new NoRightsException("Versucht Kontext aufzurufen, für das der Nutzer keine Rechte hat.");
                }
                if (rootMitgliedDetails.responseType.Equals("EXCEPTION"))
                {
                    throw new NoRightsException("Sonstiger Fehler beim Zugriff.");
                }
            }
            mitgliedDetais = rootMitgliedDetails.data;

            return mitgliedDetais;
        }
        public async Task<Ausbildung_Details> AusbildungDetails(int idAusbildung, int idMitglied)
        {

            String anfrage = "api/1/2/service/nami/mitglied-ausbildung/filtered-for-navigation/mitglied/mitglied/" + idMitglied + "/" + idAusbildung;
            String responseString = await GetApiResultStringAsync(anfrage).ConfigureAwait(false);

            Ausbildung_Details ausbildungDetails = new Ausbildung_Details();
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            RootObject_Ausbildung_Details rootAusbildung_Details = (RootObject_Ausbildung_Details)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(RootObject_Ausbildung_Details));
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

            String anfrage = "api/1/2/service/nami/grp-reports/filtered-for-grpadmin/gruppierung/crtGruppierung/" + idGruppe + "/flist";
            String responseString = await GetApiResultStringAsync(anfrage).ConfigureAwait(false);

            List<Report_Data> reportList = new List<Report_Data>();
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            Report_RootObject root_Reports = (Report_RootObject)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(Report_RootObject));
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
            Uri url;
            if (qa)
            {
                url = new Uri("https://qa.mv.meinbdp.de/ica/rest/" + anfrageURL);
            }
            else
            {
                url = new Uri("https://mv.meinbdp.de/ica/rest/" + anfrageURL);
            }
            HttpResponseMessage response = await App.client.GetAsync(url);
            //WebResponse response =  (WebResponse)request.GetResponse();
            string responseString = await response.Content.ReadAsStringAsync();
            if (debug)
            {
                Console.WriteLine(responseString);
            }
            return responseString;


        }
        private async Task<String> PostApiDataAsync(string anfrageURL, string postData)
        {

            Uri url;
            if (qa)
            {
                url = new Uri("https://qa.mv.meinbdp.de/ica/rest/" + anfrageURL);
            }
            else
            {
                url = new Uri( "https://mv.meinbdp.de/ica/rest/" + anfrageURL);
            }
            HttpResponseMessage response = await App.client.PostAsync(url,new StringContent(postData, Encoding.UTF8, "application/json"));
            //WebResponse response =  (WebResponse)request.GetResponse();
            string responseString = await response.Content.ReadAsStringAsync();
            if (debug)
            {
                Console.WriteLine(responseString);
            }
            return responseString;
        }
        private async Task<String> PutApiDataAsync(string anfrageURL, string postData)
        {
            Uri url;
            if (qa)
            {
                url = new Uri("https://qa.mv.meinbdp.de/ica/rest/" + anfrageURL);
            }
            else
            {
                url = new Uri("https://mv.meinbdp.de/ica/rest/" + anfrageURL);
            }
            HttpResponseMessage response = await App.client.PutAsync(url, new StringContent(postData, Encoding.UTF8, "application/json"));
            string responseString = await response.Content.ReadAsStringAsync();
            if (debug)
            {
                Console.WriteLine(responseString);
            }
            return responseString;
        }
        public async Task<String> PostNewMitglied(int idGruppe, string JSON)
        {

            String anfrage = "api/1/2/service/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/" + idGruppe;
            String responseString = await PostApiDataAsync(anfrage, JSON);
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            var response = (RootObj_new_Mitglied)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(RootObj_new_Mitglied));
            if (response.success == false)
            {
                if (response.responseType.Equals("ERROR") && response.message.Equals("Session expired"))
                {
                    throw new NewLoginException("Bitte neu einloggen.");
                }
                else if (response.responseType.Equals("EXCEPTION") && response.message.Equals("Benutzer darf sich keine GrpReports ansehen"))
                {
                    throw new NoRightsException("Versucht Kontext aufzurufen, für das der Nutzer keine Rechte hat.");
                }
                else
                {
                    throw new NotAllRequestedFieldsFilledException("Es ist ein Fehler aufgetreten. Wurden alle Pflichtfelder ausgefüllt?");
                }
            }


            return "Erfolgreich angelegt" + response.message;
        }
        public async Task<String> PostNewAusbildung(int idMitglied, string JSON)
        {

            String anfrage = "api/1/2/service/nami/mitglied-ausbildung/filtered-for-navigation/mitglied/mitglied/" + idMitglied;
            String responseString = await PostApiDataAsync(anfrage, JSON);
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            var response = (RootObj_new_Mitglied)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(RootObj_new_Mitglied));
            if (response.success == false)
            {
                if (response.responseType.Equals("ERROR") && response.message.Equals("Session expired"))
                {
                    throw new NewLoginException("Bitte neu einloggen.");
                }
                else if (response.responseType.Equals("EXCEPTION") && response.message.Equals("Benutzer darf sich keine GrpReports ansehen"))
                {
                    throw new NoRightsException("Versucht Kontext aufzurufen, für das der Nutzer keine Rechte hat.");
                }
                else
                {
                    throw new NotAllRequestedFieldsFilledException("Es ist ein Fehler aufgetreten. Wurden alle Pflichtfelder ausgefüllt?");
                }
            }


            return "Erfolgreich angelegt" + response.message;
        }
        public async Task<String> PutChangeAusbildung(int idMitglied, int idAusbildung, string JSON)
        {

            String anfrage = "api/1/2/service/nami/mitglied-ausbildung/filtered-for-navigation/mitglied/mitglied/" + idMitglied + "/" + idAusbildung;
            String responseString = await PutApiDataAsync(anfrage, JSON);
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            var response = (RootObj_edit_Ausbildung)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(RootObj_edit_Ausbildung));

            if (response.success == false)
            {
                if (response.responseType.Equals("ERROR") && response.message.Equals("Session expired"))
                {
                    throw new NewLoginException("Bitte neu einloggen.");
                }
                else if (response.responseType.Equals("EXCEPTION") && response.message.Equals("Benutzer darf sich keine GrpReports ansehen"))
                {
                    throw new NoRightsException("Versucht Kontext aufzurufen, für das der Nutzer keine Rechte hat.");
                }
                else
                {
                    throw new NotAllRequestedFieldsFilledException("Es ist ein Fehler aufgetreten. Wurden alle Pflichtfelder ausgefüllt?");
                }
            }


            return "Erfolgreich geändert" + response.message;
        }
        public async Task<String> PutChangeMitglied(int idGruppe, int idMitglied, string JSON)
        {

            String anfrage = "api/1/2/service/nami/mitglied/filtered-for-navigation/gruppierung/gruppierung/" + idGruppe + "/" + idMitglied;
            String responseString = await PutApiDataAsync(anfrage, JSON);
            APIResponse aPIResponse = JsonConvert.DeserializeObject<APIResponse>(responseString);
            JsonSerializer jSerializer = new JsonSerializer();
            var response = (RootObj_edit_Mitglied)jSerializer.Deserialize(new JTokenReader(aPIResponse.response), typeof(RootObj_edit_Mitglied));

            if (response.success == false)
            {
                if (response.responseType.Equals("ERROR") && response.message.Equals("Session expired"))
                {
                    throw new NewLoginException("Bitte neu einloggen.");
                }
                else if (response.responseType.Equals("EXCEPTION") && response.message.Equals("Benutzer darf sich keine GrpReports ansehen"))
                {
                    throw new NoRightsException("Versucht Kontext aufzurufen, für das der Nutzer keine Rechte hat.");
                }
                else
                {
                    throw new NotAllRequestedFieldsFilledException("Es ist ein Fehler aufgetreten. Wurden alle Pflichtfelder ausgefüllt?");
                }
            }

            return "Erfolgreich geändert" + response.message;

        }



    }
}
