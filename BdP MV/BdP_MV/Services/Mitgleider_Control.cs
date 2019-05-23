using BdP_MV.Model;
using BdP_MV.Model.Mitglied;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BdP_MV.Services
{
    public class Mitglieder_Control

    {
        private MainController mainC;
        private List<Mitglied> alleMitglieder;
        private List<Mitglied> aktiveMitglieder;

        public List<Mitglied> AlleMitglieder { get => alleMitglieder; set => alleMitglieder = value; }
        public List<Mitglied> AktiveMitglieder { get => aktiveMitglieder; set => aktiveMitglieder = value; }

        public Mitglieder_Control(MainController mainCo)
        {
            mainC = mainCo;
        }
        private void MitgliederNachbearbeiten()
        {
            AktiveMitglieder = new List<Mitglied>();
            if (AlleMitglieder != null)
            {
                foreach (Mitglied aktuellesMitglied in AlleMitglieder)
                {

                    if (aktuellesMitglied.entries_status.Equals("Aktiv"))
                    {
                        //aktuellesMitglied.alleTaetigkeiten = mainC.MvConnector.Taetigkeiten(aktuellesMitglied.id);
                        //aktuellesMitglied.Gruppe = GruppennameHerausfinden(aktuellesMitglied.alleTaetigkeiten);
                        AktiveMitglieder.Add(aktuellesMitglied);

                    }
                    else if (aktuellesMitglied.entries_status.Equals("Wartend"))
                    {
                        AktiveMitglieder.Add(aktuellesMitglied);
                    }
                    aktuellesMitglied.ansprechname = ChooseAnsprechname(aktuellesMitglied);
                }


                AktiveMitglieder = MitgliederSort(AktiveMitglieder);


            }

        }
        private List<Mitglied> MitgliederNachbearbeiten(List<Mitglied> mitglieder)
        {
            if (mitglieder != null)
            {
                Parallel.ForEach(mitglieder, (currentmitglied) =>
                {
                    currentmitglied.ansprechname = ChooseAnsprechname(currentmitglied);
                });

                mitglieder = MitgliederSort(mitglieder);
            }
            return mitglieder;

        }
        private List<Mitglied> MitgliederSort (List<Mitglied> mitglieder)
        {
            if (mainC.einsteillungen.sortierreihenfolge == 1)
            {
                mitglieder.OrderByDescending(mitglied => mitglied.entries_nachname).ThenBy(mitglied => mitglied.entries_vorname);
            }
            else if (mainC.einsteillungen.sortierreihenfolge == 2)
            {
                mitglieder.OrderBy(mitglied => mitglied.entries_vorname).ThenBy(mitglied => mitglied.entries_nachname);
            }
            else if (mainC.einsteillungen.sortierreihenfolge == 3)
            {
                mitglieder.OrderBy(mitglied => mitglied.ansprechname).ThenBy(mitglied => mitglied.entries_nachname);

            }
            return mitglieder;
        }
        private static String ChooseAnsprechname(Mitglied mitglied)
        {
            string ansprechname;
            if (String.IsNullOrEmpty(mitglied.entries_spitzname))
            {
                ansprechname = mitglied.entries_vorname;
            }
            else
            {
                ansprechname = mitglied.entries_spitzname;
            }
            return ansprechname;

        }
        private static String ChooseAnsprechname(MitgliedDetails mitglied)
        {
            string ansprechname;
            if (String.IsNullOrEmpty(mitglied.spitzname))
            {
                ansprechname = mitglied.vorname;
            }
            else
            {
                ansprechname = mitglied.spitzname;
            }
            return ansprechname;

        }
        private static int GetAgeFromDate(DateTime birthday)
        {
            int years = DateTime.Now.Year - birthday.Year;
            birthday = birthday.AddYears(years);
            if (DateTime.Now.CompareTo(birthday) < 0) { years--; }
            return years;
        }
        public async Task<MitgliedDetails> MitgliedDetailsAbrufen(int idMitglied, int idGruppe)
        {
            MitgliedDetails mitglied = new MitgliedDetails();
            mitglied = await mainC.mVConnector.MitgliedDetails(idMitglied, idGruppe);

            mitglied.ansprechname = ChooseAnsprechname(mitglied);
            try
            {
                DateTime geburtsDatum = (DateTime)mitglied.geburtsDatum;
                mitglied.alter = GetAgeFromDate(geburtsDatum);
            }
            catch (Exception e)
            {
                mitglied.alter = 0;
            }
            if (mitglied.geschlechtId == null)
            {
                mitglied.geschlechtId = 3;
                mitglied.geschlecht = "n/a";
            }
            if (mitglied.landId == null)
            {
                mitglied.landId = 1;
                mitglied.land = "Deutschland";
            }
            return mitglied;


        }
        public async Task<List<Taetigkeit>> TaetigkeitenAbrufen(int idMitglied)
        {
            List<Taetigkeit> taetigkeiten = new List<Taetigkeit>();
            taetigkeiten = await mainC.mVConnector.Taetigkeiten(idMitglied).ConfigureAwait(false);
            Regex reg_taetigkeitsname = new Regex(@"\s+\(([0-9]*|[,]*|[.]*)*\)\s*(\[[A-Z]\])*");
            Regex reg_gruppe = new Regex(@"(\s)*([0-9]+)");
            //foreach (Taetigkeit t in taetigkeiten)
            Parallel.ForEach(taetigkeiten, (t) =>
            {
                t.entries_taetigkeit = reg_taetigkeitsname.Replace(t.entries_taetigkeit, "$1");
                t.entries_gruppierung = reg_gruppe.Replace(t.entries_gruppierung, "$1");

                //überprüfen ob die Tätigkeit aktiv ist.
                if (t.entries_aktivBis.HasValue)
                {

                    if (DateTime.Compare((DateTime)t.entries_aktivBis, DateTime.Now) <= 0)
                    {
                        t.aktiv = false;
                    }
                    else
                    {
                        t.aktiv = true;
                    }
                }
                else
                {
                    t.aktiv = true;
                }
            });

            taetigkeiten = taetigkeiten.OrderBy(o => o.entries_taetigkeit).ToList();

            taetigkeiten = taetigkeiten.OrderByDescending(o => o.aktiv).ToList();
            return taetigkeiten;
        }
        public async Task<List<Ausbildung>> AusbildungenAbrufen(int idMitglied)
        {
            List<Ausbildung> ausbildungen = new List<Ausbildung>();
            ausbildungen = await mainC.mVConnector.Ausbildung(idMitglied);
            ausbildungen = ausbildungen.OrderByDescending(o => o.entries_vstgTag).ToList();
            return ausbildungen;
        }
        public async Task<List<SGB8>> Sgb8Abrufen(int idMitglied)
        {
            List<SGB8> fuehrungszeugnisse = new List<SGB8>();
            fuehrungszeugnisse = await mainC.mVConnector.SGB8(idMitglied);
            fuehrungszeugnisse = fuehrungszeugnisse.OrderByDescending(o => o.entries_fzDatum).ToList();

            return fuehrungszeugnisse;
        }
        public String latestSGB8(List<SGB8> fuehrungszeugnisse)
        {
            String returnString;
            CultureInfo ci = new CultureInfo("de-DE");

            if (fuehrungszeugnisse.Count==0)
            {
                returnString = "Kein Führungszeungnis eingesehen";
            }
            else
            {
                DateTime letztesFZ = (DateTime)fuehrungszeugnisse.First<SGB8>().entries_fzDatum;
               
                returnString = "Letztes FZ ist " + GetAgeFromDate(letztesFZ) + " Jahre alt. (Datum: " + letztesFZ.ToString("d", ci) + ")";
            }
            return returnString;
        }

        public async Task MitgliederAktualisierenByGroup()
        {
            //AlleMitglieder = await Task<List<Mitglied>>.Run(() => mainC.MvConnector.Mitglieder(mainC.gruppencontroller.AktuelleGruppe, true));


            AlleMitglieder = await mainC.mVConnector.Mitglieder(mainC.einsteillungen.aktuelleGruppe, true);
            MitgliederNachbearbeiten();

            Console.WriteLine("Mitglieder_Gefiltert");

        }
        public async Task<List<Mitglied>> MitgliederAbrufenBySearch(SearchObject suchObjekt)
        {

            suchObjekt.searchType = "MITGLIEDER";
            JsonSerializerSettings settingsJSON = new JsonSerializerSettings();
            //settingsJSON.NullValueHandling = NullValueHandling.Ignore;
            
            string suchobjektJSON = JsonConvert.SerializeObject(suchObjekt, Formatting.Indented, settingsJSON);
            suchobjektJSON = suchobjektJSON.Replace("null", "\"\"");
            suchobjektJSON = Regex.Replace(suchobjektJSON, @"\t|\n|\r", "");

           // suchobjektJSON = Regex.Replace(suchobjektJSON, @"^""|""$|\\n?", "");
            suchobjektJSON = suchobjektJSON.Replace(" ", "");
            suchobjektJSON = suchobjektJSON.Replace(@"\", @"");
            //suchobjektJSON = suchobjektJSON.Substring(1, suchobjektJSON.Length - 1);
            Console.WriteLine(suchobjektJSON);
            //suchobjektJSON = Regex.Unescape(suchobjektJSON);

            List<Mitglied> mitglieder=await mainC.mVConnector.Mitglieder(suchobjektJSON);
            mitglieder = MitgliederNachbearbeiten(mitglieder);

            Console.WriteLine("Mitglieder_Gefiltert");
            return mitglieder;

        }
        public String GruppennameHerausfinden(List<Taetigkeit> taetigkeiten)
        {
            string gruppe = "";
            foreach (Taetigkeit aktuellTaetigkeit in taetigkeiten)
            {
                if (aktuellTaetigkeit.aktiv)
                {
                    if (aktuellTaetigkeit.entries_taetigkeit.StartsWith("."))
                    {
                        gruppe = aktuellTaetigkeit.entries_taetigkeit;
                        string[] meineStrings = gruppe.Split(new Char[] { '(' });
                        gruppe = meineStrings[0];
                        //Regex regex = new Regex(".");
                        //gruppe = regex.Replace(gruppe, String.Empty);
                        gruppe = gruppe.TrimStart('.');
                        return gruppe;
                    }
                }
            }
            return gruppe;

            }
           
        }
}
