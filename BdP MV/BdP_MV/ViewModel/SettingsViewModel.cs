using BdP_MV.Model.Settings;
using BdP_MV.Services;
using MvvmHelpers;
using System;

/* Nicht gemergte Änderung aus Projekt "BdP_MV.iOS"
Vor:
using System.Windows.Input;

using Xamarin.Forms;
using System.Collections.Generic;
Nach:
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Generic;
*/
using System.Collections.Essentials;
using Xamarin.Forms;

namespace BdP_MV.ViewModel
{
    public class SettingsViewModel : BaseNavigationViewModel
    {
        public List<SettingKeyValue> Sortierreihenfolgen { get; set; } = new List<SettingKeyValue>()
    {
        new SettingKeyValue(){Name = "Nachname, Vorname",Value = 1},
        new SettingKeyValue(){Name = "Vorname, Nachname",Value = 2},
        new SettingKeyValue(){Name = "Spitz-/Vorname, Nachname",Value = 3}
    };

/* Nicht gemergte Änderung aus Projekt "BdP_MV.iOS"
Vor:
        public MainController mainC = new MainController();
        
        public Boolean loadKleingruppen { get; set; }
Nach:
        public MainController mainC = new MainController();

        public Boolean loadKleingruppen { get; set; }
*/
        public MainController mainC = new MainController();

        public Boolean loadKleingruppen { get; set; }
        public Boolean inaktiveAnzeigen { get; set; }
        public int sortierreihenfolge { get; set; }

        private SettingKeyValue _sortierreihenfolge;
        public SettingKeyValue Sortierreihenfolge
        {
            get
            {
                return _sortierreihenfolge;
            }
            set
            {
                _sortierreihenfolge = value;
                OnPropertyChanged();
            }
        }

        public SettingsViewModel()
        {
            loadKleingruppen = true;

            sortierreihenfolge = Preferences.Get("sortierreihenfolge", 1);
            loadKleingruppen = Preferences.Get("loadKleingruppen", true);
            inaktiveAnzeigen = Preferences.Get("inaktiveAnzeigen", false);

        }
        public void EinstellungenAnwenden()
        {
            Preferences.Set("sortierreihenfolge", sortierreihenfolge);
            Preferences.Set("loadKleingruppen", loadKleingruppen);
            Preferences.Set("inaktiveAnzeigen", inaktiveAnzeigen);



        }

    }
}