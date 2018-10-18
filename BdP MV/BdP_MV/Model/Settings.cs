using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BdP_MV.Model
{
    public class Settings
    {
        public Boolean loadKleingruppen { get; set; }
        public int aktuelleGruppe { get; set; }
        public int sortierreihenfolge { get; set; } //1: Nachname, Vorname; 2: Vorname, Nachname; 3: Ansprechname, Nachname
        public Settings()
        {
            if (Application.Current.Properties.ContainsKey("loadKleingruppen"))
            {
                 loadKleingruppen = (Boolean)Application.Current.Properties["loadKleingruppen"] ;
            }
            if (Application.Current.Properties.ContainsKey("sortierreihenfolge"))
            {
                sortierreihenfolge = (int)Application.Current.Properties["sortierreihenfolge"];
            }
            else
            {
                sortierreihenfolge = 1;
            }
                

        }
    }
}
