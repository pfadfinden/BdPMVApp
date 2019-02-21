using BdP_MV.Model;
using BdP_MV.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.ViewModel
{
    public class NewMitgliedViewModel : BaseNavigationViewModel
    {
        public MainController mainC;
        public SelectableItem geschlechter;
        public SelectableItem beitragsart;
        public SelectableItem mitgltype;
        public SelectableItem land;

        public NewMitgliedViewModel()
        {
            mainC = new MainController();


        }
        
    }
}
