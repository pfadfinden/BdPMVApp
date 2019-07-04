using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BdP_MV.Model.Settings
{
    public class Einstellungen
    {
        public Boolean loadKleingruppen { get; set; }
        public int aktuelleGruppe { get; set; }
        public int sortierreihenfolge { get; set; } //1: Nachname, Vorname; 2: Vorname, Nachname; 3: Ansprechname, Nachname
        public Einstellungen()
        {
            if (Application.Current.Properties.ContainsKey("settings"))
            {
                 Einstellungen loadsetting = (Einstellungen)Application.Current.Properties["settings"] ;
                this.loadKleingruppen = loadsetting.loadKleingruppen;
                this.aktuelleGruppe = loadsetting.aktuelleGruppe;
                this.sortierreihenfolge = loadsetting.sortierreihenfolge;
            }
          else
            { sortierreihenfolge = 1;
                loadKleingruppen = true;
                aktuelleGruppe = 0;
                Application.Current.Properties["settings"] = this;
            }

        }
    }
}
