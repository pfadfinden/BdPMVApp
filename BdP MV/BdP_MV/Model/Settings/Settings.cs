using System;
using Xamarin.Forms;

namespace BdP_MV.Model.Settings
{
    public class Einstellungen
    {
        public Boolean loadKleingruppen { get; set; }
        public Boolean inaktiveAnzeigen { get; set; }
        public int aktuelleGruppe { get; set; }
        public int sortierreihenfolge { get; set; } //1: Nachname, Vorname; 2: Vorname, Nachname; 3: Ansprechname, Nachname
        public Einstellungen()
        {
            if (Application.Current.Properties.ContainsKey("settings"))
            {
                Einstellungen loadsetting = (Einstellungen)Application.Current.Properties["settings"];
                loadKleingruppen = loadsetting.loadKleingruppen;
                aktuelleGruppe = loadsetting.aktuelleGruppe;
                sortierreihenfolge = loadsetting.sortierreihenfolge;
                inaktiveAnzeigen = loadsetting.inaktiveAnzeigen;
            }
            else
            {
                sortierreihenfolge = 1;
                loadKleingruppen = true;
                aktuelleGruppe = 0;
                inaktiveAnzeigen = false;
                Application.Current.Properties["settings"] = this;
            }

        }
    }
}
