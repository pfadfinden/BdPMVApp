using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace BdP_MV.View.MasterDetail
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetail_Main : MasterDetailPage
    {
         MainController mainC;


        public MasterDetail_Main(MainController mainCo)
        {
            InitializeComponent();
            mainC = mainCo;
            MasterPage.ListView.ItemSelected += ListView_ItemSelected;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var item = e.SelectedItem as MasterDetail_MainMenuItem;
            if (item == null)
                return;
            Page page;
            if (item.TargetType == typeof(Suche)|| item.TargetType == typeof(ItemsPage))
            {
                page = (Page)Activator.CreateInstance(item.TargetType, mainC);
            }
            else
            {
                page = (Page)Activator.CreateInstance(item.TargetType);
            }
            page.Title = item.Title;
            Detail = new NavigationPage(page);
            
            IsPresented = false;

            MasterPage.ListView.SelectedItem = null;
        }
    }
}