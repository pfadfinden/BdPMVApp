using BdP_MV.Model;
using System;
using System.Collections.Generic;
using System.Text;
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
        public void AlleGruppenAbrufen(int id)
        {
            List<Gruppe> tempGruppen = new List<Gruppe>();
            tempGruppen = mainC.mVConnector.GetGroups(id);
            if (tempGruppen.Count > 0)
                foreach (Gruppe aktGruppe in tempGruppen)
                {
                    alleGruppen.Add(aktGruppe);
                    AlleGruppenAbrufen(aktGruppe.id);
                }

        }

    }
}
