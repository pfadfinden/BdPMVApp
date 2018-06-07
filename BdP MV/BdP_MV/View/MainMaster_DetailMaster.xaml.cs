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

namespace BdP_MV.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MainMaster_DetailMaster : ContentPage
    {
        public ListView ListView;

        public MainMaster_DetailMaster()
        {
            InitializeComponent();

            BindingContext = new MainMaster_DetailMasterViewModel();
            ListView = MenuItemsListView;
        }

        class MainMaster_DetailMasterViewModel : INotifyPropertyChanged
        {
            public ObservableCollection<MainMaster_DetailMenuItem> MenuItems { get; set; }
            
            public MainMaster_DetailMasterViewModel()
            {
                MenuItems = new ObservableCollection<MainMaster_DetailMenuItem>(new[]
                {
                    new MainMaster_DetailMenuItem { Id = 0, Title = "Page 1" },
                    new MainMaster_DetailMenuItem { Id = 1, Title = "Page 2" },
                    new MainMaster_DetailMenuItem { Id = 2, Title = "Page 3" },
                    new MainMaster_DetailMenuItem { Id = 3, Title = "Page 4" },
                    new MainMaster_DetailMenuItem { Id = 4, Title = "Page 5" },
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