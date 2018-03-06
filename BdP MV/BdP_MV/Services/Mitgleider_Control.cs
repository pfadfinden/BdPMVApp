using BdP_MV.Model;
using System;
using System.Collections.Generic;
using System.Text;
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
                AktiveMitglieder.Sort((p1, p2) => p1.entries_nachname.CompareTo(p2.entries_nachname));

            }

        }
        private String ChooseAnsprechname(Mitglied mitglied)
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
        private String ChooseAnsprechnameDetails(MitgliedDetails mitglied)
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
        private int GetAgeFromDate(DateTime birthday)
        {
            int years = DateTime.Now.Year - birthday.Year;
            birthday = birthday.AddYears(years);
            if (DateTime.Now.CompareTo(birthday) < 0) { years--; }
            return years;
        }
        public async Task<MitgliedDetails> MitgliedDetailsAbrufen(int id)
        {
            MitgliedDetails mitglied = new MitgliedDetails();
            mitglied =  await mainC.mVConnector.MitgliedDetails(id);

            mitglied.ansprechname = ChooseAnsprechnameDetails(mitglied);
            try
            {
                DateTime geburtsDatum = (DateTime)mitglied.geburtsDatum;
                mitglied.alter = GetAgeFromDate(geburtsDatum);
            }
            catch (Exception e)
            {
                mitglied.alter = 0;
            }

            return mitglied;


        }

        public async Task MitgliederAktualisierenByGroup()
        {
            //AlleMitglieder = await Task<List<Mitglied>>.Run(() => mainC.MvConnector.Mitglieder(mainC.gruppencontroller.AktuelleGruppe, true));


            AlleMitglieder =  await mainC.mVConnector.Mitglieder(mainC.einsteillungen.aktuelleGruppe, true);
            MitgliederNachbearbeiten();

            Console.WriteLine("Mitglieder_Gefiltert");

        }
        //private String GruppennameHerausfinden(List<Taetigkeit> taetigkeiten)
        //{
        //    string gruppe = "";
        //    foreach (Taetigkeit aktuellTaetigkeit in taetigkeiten)
        //    {
        //        if (aktuellTaetigkeit.entries_aktivBis == "")
        //        {
        //            if (aktuellTaetigkeit.entries_taetigkeit.StartsWith("."))
        //            {
        //                gruppe = aktuellTaetigkeit.entries_taetigkeit;
        //                string[] meineStrings = gruppe.Split(new Char[] { '(' });
        //                gruppe = meineStrings[0];
        //                //Regex regex = new Regex(".");
        //                //gruppe = regex.Replace(gruppe, String.Empty);
        //                gruppe = gruppe.TrimStart('.');
        //                return gruppe;
        //            }
        //        }
        //    }
        //    return gruppe;

        //}
        //public MitgliedDetails MitgliedDetailsFinden(int id)
        //{
        //    Mitglied mitgliedAusUebersicht = alleMitglieder.Find(x => x.entries_id == id);
        //    String gruppe = mitgliedAusUebersicht.Gruppe;
        //    MitgliedDetails mitgliedDetail = mainC.mVConnector.MitgliedDetails(id);
        //    mitgliedDetail.gruppe = gruppe;
        //    mitgliedDetail.alter = GetAgeFromDate(mitgliedDetail.geburtsDatum);
        //    return mitgliedDetail;
        //}
    }
}
