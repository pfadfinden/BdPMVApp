using BdP_MV.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BdP_MV.Services
{
    public class Group_Control
    {
        public List<Gruppe> alleGruppen;
        private MainController mainC;
        public Group_Control(MainController mainCo)
        {
            mainC = mainCo;
            alleGruppen = new List<Gruppe>();
        }
       //
        public async Task AlleGruppenAbrufen(int id, string prefix)
        {
            List<Gruppe> tempGruppen = new List<Gruppe>();
            tempGruppen = mainC.mVConnector.GetGroups(id);
            Regex reg = new Regex(@"(\s)*([0-9]+)");
            if (tempGruppen.Count > 0)
                foreach (Gruppe aktGruppe in tempGruppen)
                {
                    
                    aktGruppe.descriptor = reg.Replace(aktGruppe.descriptor, "$1");
                    aktGruppe.descriptor = prefix + aktGruppe.descriptor;
                    alleGruppen.Add(aktGruppe);
                    await AlleGruppenAbrufen(aktGruppe.id, prefix+"-");
                }

        }

    }
}
