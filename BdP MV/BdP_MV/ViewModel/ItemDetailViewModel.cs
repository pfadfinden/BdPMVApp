
using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace BdP_MV.ViewModel
{
    public class ItemDetailViewModel : BaseNavigationViewModel
    {
        public ItemDetailViewModel(MitgliedDetails mitgliedDetails, MainController mainCo)
        {
            mainC = mainCo;

            mitglied = mitgliedDetails;

        }

        public MainController mainC;

        public bool HasZusatzAdresse => !string.IsNullOrWhiteSpace(mitglied?.nameZusatz);
        public MitgliedDetails mitglied { private set; get; }
        public List<Taetigkeit> taetigkeiten { set; get; }
        public List<Taetigkeit> taetigkeitenAktiv { set; get; }
        public List<SGB8> sgb8 { set; get; }
        public List<Ausbildung> ausbildung { set; get; }
        public String latestSGB8 { set; get; }
        public bool isEditable { set; get; }


        public bool HasPhoneNumber => !string.IsNullOrWhiteSpace(mitglied?.telefon1);
        public bool HasCellphoneNumber => !string.IsNullOrWhiteSpace(mitglied?.telefon3);
        public bool HasKleingruppe { private set; get; }

        public bool HasAddress => true;
        public bool HasEmailAddress => !string.IsNullOrWhiteSpace(mitglied?.email);
        public bool HasParentEmailAddress => !string.IsNullOrWhiteSpace(mitglied?.emailVertretungsberechtigter);
        public bool IsEditable = true;



        // this is just a utility service that we're using in this demo app to mitigate some limitations of the iOS simulator



        public async Task Nachbearbeitung()
        {
            Task<String> t1 = Task<String>.Run(() => mainC.mitgliederController.latestSGB8(sgb8));
            Boolean loadKleingruppen=Preferences.Get("loadKleingruppen", true);
            if (loadKleingruppen)
            {

                Task<String> t2 = Task<String>.Run(() => mainC.mitgliederController.GruppennameHerausfinden(taetigkeiten));
                mitglied.kleingruppe = await t2;
            }
            else
            {
                mitglied.kleingruppe = "Es werden keine Kleingruppen geladen. Bitte in den Einstellungen auswählen.";
            }
            latestSGB8 = await t1;
            
            HasKleingruppe = !string.IsNullOrWhiteSpace(mitglied?.kleingruppe);
            TaetigkeitenFilter();



        }

        Command _DialNumberCommand;

        private void TaetigkeitenFilter()
        {
            taetigkeitenAktiv = taetigkeiten.Where(x => x.aktiv).ToList();
        }
        public Command DialNumberCommand => _DialNumberCommand ??
                                            (_DialNumberCommand = new Command(ExecuteDialNumberCommand));

        void ExecuteDialNumberCommand()
        {
            if (string.IsNullOrWhiteSpace(mitglied.telefon1))
                return;
            else
            {
                Device.OpenUri(new Uri("tel:" + mitglied.telefon1.ToString()));
            }


        }

        Command _MessageNumberCommand;

        public Command MessageNumberCommand => _MessageNumberCommand ??
                                               (_MessageNumberCommand = new Command(ExecuteMessageNumberCommand));

        void ExecuteMessageNumberCommand()
        {
            if (string.IsNullOrWhiteSpace(mitglied.telefon1))
                return;


        }
        Command _DialCellphoneNumberCommand;

        public Command DialCellphoneNumberCommand => _DialCellphoneNumberCommand ??
                                            (_DialCellphoneNumberCommand = new Command(ExecuteDialCellphoneNumberCommand));

        void ExecuteDialCellphoneNumberCommand()
        {
            if (string.IsNullOrWhiteSpace(mitglied.telefon3))
                return;
            else
            {

                Device.OpenUri(new Uri("tel:" + mitglied.telefon3.ToString()));
            }

        }

        Command _MessageCellphoneNumberCommand;

        public Command MessageCellphoneNumberCommand => _MessageNumberCommand ??
                                               (_MessageNumberCommand = new Command(ExecuteCellphoneMessageNumberCommand));

        void ExecuteCellphoneMessageNumberCommand()
        {
            if (string.IsNullOrWhiteSpace(mitglied.telefon3))
                return;
            else
            {
                Device.OpenUri(new Uri("sms:" + mitglied.telefon3.ToString()));
            }

        }
        Command _EmailCommand;

        public Command EmailCommand => _EmailCommand ??
                                       (_EmailCommand = new Command(ExecuteEmailCommandCommand));

        void ExecuteEmailCommandCommand()
        {
            if (string.IsNullOrWhiteSpace(mitglied.email))
                return;
            else
            {
                Device.OpenUri(new Uri("mailto:" + mitglied.email));
            }


        }
        Command _ParentEmailCommand;

        public Command ParentEmailCommand => _ParentEmailCommand ??
                                       (_ParentEmailCommand = new Command(ExecuteParentEmailCommandCommand));

        void ExecuteParentEmailCommandCommand()
        {
            if (string.IsNullOrWhiteSpace(mitglied.emailVertretungsberechtigter))
                return;
            else
            {
                Device.OpenUri(new Uri("mailto:" + mitglied.emailVertretungsberechtigter));
            }

        }





        public void DisplayGeocodingError()
        {
            //MessagingService.Current.SendMessage<MessagingServiceAlert>(MessageKeys.DisplayAlert, new MessagingServiceAlert()
            //    {
            //        Title = "Geocoding Error", 
            //        Message = "Please make sure the address is valid, or that you have a network connection.",
            //        Cancel = "OK"
            //    });

            IsBusy = false;
        }




        static bool AddressBeginsWithNumber(string address)
        {
            return !string.IsNullOrWhiteSpace(address) && char.IsDigit(address.ToCharArray().First());
        }



        static int GetEndingIndexOfNumericPortionOfAddress(string address)
        {
            int endingIndex = 0;

            for (int i = 0; i < address.Length; i++)
            {
                if (char.IsDigit(address[i]))
                    endingIndex = i;
                else
                    break;
            }

            return endingIndex;
        }
        public async Task<Ausbildung_Details> getAusbildungDetails(int selectedAusbildung)
        {
            IsBusy = true;
            Ausbildung_Details loadedDetails = await mainC.mVConnector.AusbildungDetails(selectedAusbildung, mitglied.id);
            IsBusy = false;
            return loadedDetails;

        }
    }
}

