using BdP_MV.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.Services
{
    class Mitgleider_Control

    {
        private MainController mainC;
        private List<Mitglied> alleMitglieder;
        private List<Mitglied> aktiveMitglieder;

        public List<Mitglied> AlleMitglieder { get => alleMitglieder; set => alleMitglieder = value; }
        public List<Mitglied> AktiveMitglieder { get => aktiveMitglieder; set => aktiveMitglieder = value; }

        public Mitgleider_Control(MainController mainCo)
        {
            mainC = mainCo;
        }
        private void MitgliederNachbearbeiten()
        {
            AktiveMitglieder = new List<Mitglied>();
            if (AlleMitglieder != null)
            {
                Parallel.ForEach(AlleMitglieder, (aktuellesMitglied) =>
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
                });
            }

        }
        public async Task MitgliederAktualisierenByGroup()
        {
            //AlleMitglieder = await Task<List<Mitglied>>.Run(() => mainC.MvConnector.Mitglieder(mainC.gruppencontroller.AktuelleGruppe, true));


            AlleMitglieder = await mainC.mVConnector.Mitglieder(mainC.einsteillungen.aktuelleGruppe, true);
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
        public static int GetAgeFromDate(DateTime birthday)
        {
            int years = DateTime.Now.Year - birthday.Year;
            birthday = birthday.AddYears(years);
            if (DateTime.Now.CompareTo(birthday) < 0) { years--; }
            return years;
        }
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
