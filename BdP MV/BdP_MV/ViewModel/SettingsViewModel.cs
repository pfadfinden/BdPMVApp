using BdP_MV.Model.Settings;
using MvvmHelpers;
using BdP_MV.Services;
using System;
using System.Windows.Input;

using Xamarin.Forms;
using System.Collections.Generic;

namespace BdP_MV.ViewModel
{
    public class SettingsViewModel : BaseViewModel
    {
        public List<SettingKeyValue> Sortierreihenfolgen { get; set; } = new List<SettingKeyValue>()
    {
        new SettingKeyValue(){Name = "Nachname, Vorname",Value = 1},
        new SettingKeyValue(){Name = "Vorname, Nachname",Value = 2},
        new SettingKeyValue(){Name = "Ansprechname, Nachname",Value = 3}
    };
        public MainController mainC = new MainController();
        public Einstellungen settings;

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
            Title = "Einstellungen";
            settings = mainC.einsteillungen;

        }
        public async void EinstellungenAnwenden()
        {
            mainC.einsteillungen = settings;
            Application.Current.Properties["settings"] = mainC.einsteillungen;
            

        }

    }
}