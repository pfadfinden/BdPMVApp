
using BdP_MV.Model.Mitglied;
using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
    
        MainController mainC;

        public bool HasZusatzAdresse;
        public MitgliedDetails mitglied { private set; get; }
        public List<Taetigkeit> taetigkeiten {set; get; }
        public List<SGB8> sgb8 { set; get; }
        public List<Ausbildung> ausbildung { set; get; }

        public bool HasPhoneNumber => !string.IsNullOrWhiteSpace(mitglied?.telefon1);
        public bool HasCellphoneNumber => !string.IsNullOrWhiteSpace(mitglied?.telefon3);
        public bool HasKleingruppe;

        public bool HasAddress => true;
        public bool HasEmailAddress => !string.IsNullOrWhiteSpace(mitglied?.email);
        public bool HasParentEmailAddress => !string.IsNullOrWhiteSpace(mitglied?.emailVertretungsberechtigter);
        public bool IsEditable = true;


        // this is just a utility service that we're using in this demo app to mitigate some limitations of the iOS simulator



        public void Nachbearbeitung()
        {
            mitglied.kleingruppe = mainC.mitgliederController.GruppennameHerausfinden(taetigkeiten);
            HasKleingruppe = !string.IsNullOrWhiteSpace(mitglied?.kleingruppe);

    }
        Command _EditAcquaintanceCommand;

        public Command EditAcquaintanceCommand
        {
            get
            {
                return _EditAcquaintanceCommand ??
                    (_EditAcquaintanceCommand = new Command(async () => await ExecuteEditAcquaintanceCommand()));
            }
        }

        async Task ExecuteEditAcquaintanceCommand()
        {
            //await PushAsync(new AcquaintanceEditPage() { BindingContext = new AcquaintanceEditViewModel(Acquaintance) });
        }

        Command _DeleteAcquaintanceCommand;

        public Command DeleteAcquaintanceCommand => _DeleteAcquaintanceCommand ?? (_DeleteAcquaintanceCommand = new Command(ExecuteDeleteAcquaintanceCommand));

        void ExecuteDeleteAcquaintanceCommand()
        {
            
        }

        Command _DialNumberCommand;

        public Command DialNumberCommand => _DialNumberCommand ??
                                            (_DialNumberCommand = new Command(ExecuteDialNumberCommand));

        void ExecuteDialNumberCommand()
        {
            if (string.IsNullOrWhiteSpace(mitglied.telefon1))
                return;
            else
            {
                Device.OpenUri(new Uri("tel:"+mitglied.telefon1.ToString()));
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

    }
}

