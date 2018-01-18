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
        }
        public async Task GetAlleGruppen(int id)
        {
            
                List<Gruppe> tempGruppen = new List<Gruppe>();
                tempGruppen = await mainC.mVConnector.GetGroups(id);
                if (tempGruppen.Count > 0)
                    foreach (Gruppe aktGruppe in tempGruppen)
                    {
                        alleGruppen.Add(aktGruppe);
                    await GetAlleGruppen(aktGruppe.id);
                    }

        }

    }
}
