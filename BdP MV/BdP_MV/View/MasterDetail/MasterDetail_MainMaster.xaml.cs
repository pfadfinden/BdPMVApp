using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetail_MainMaster : ContentPage
    {
        public ListView ListView;

        public MasterDetail_MainMaster()
        {
            InitializeComponent();

            BindingContext = new MasterDetail_MainMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MasterDetail_MainMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MasterDetail_MainMenuItem> MenuItems { get; set; }
            
            public MasterDetail_MainMasterViewModel()
            {
                MenuItems = new ObservableCollection<MasterDetail_MainMenuItem>(new[]
                {
                    new MasterDetail_MainMenuItem { Id = 0, Title = "Mitgliederliste",TargetType=typeof(ItemsPage) },
                    new MasterDetail_MainMenuItem { Id = 1, Title = "Suche" ,TargetType=typeof(Suche) },
                    new MasterDetail_MainMenuItem { Id = 2, Title = "Einstellungen",TargetType=typeof(Einstellungen)}
                });
            }
            
            #region INotifyPropertyChanged Implementation
            public event PropertyChangedEventHandler PropertyChanged;
            void OnPropertyChanged([CallerMemberName] string propertyName = "")
            {
                if (PropertyChanged == null)
                    return;

                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
            #endregion
        }
    }
}