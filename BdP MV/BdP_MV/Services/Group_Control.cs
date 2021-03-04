using BdP_MV.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private Boolean isGroupEndOfTree(Gruppe gruppe)
        {
            try
            {
                String descriptor = gruppe.descriptor;
                bool b = descriptor.EndsWith("00");
                return (!b);
            }
            catch (ArgumentNullException)
            {
                return false;
            }
        }

        public async Task AlleGruppenAbrufen(int id, string prefix)
        {
            List<Gruppe> tempGruppen = new List<Gruppe>();
            tempGruppen = await mainC.mVConnector.GetGroups(id);
            Regex reg = new Regex(@"(\s)*([0-9]+)");
            if (tempGruppen.Count > 0)
            {
                foreach (Gruppe aktGruppe in tempGruppen)
                {
                    bool isEndofTree = isGroupEndOfTree(aktGruppe);
                    aktGruppe.descriptor = reg.Replace(aktGruppe.descriptor, "$1");
                    aktGruppe.descriptor = prefix + aktGruppe.descriptor;
                    alleGruppen.Add(aktGruppe);
                    if (!isEndofTree)
                    {
                        await AlleGruppenAbrufen(aktGruppe.id, prefix + "- ").ConfigureAwait(false);
                    }
                }
            }
        }
        public async Task<Boolean> CheckPermissionForNew(int idGruppe)
        {
            Meta_Data meta = await mainC.mVConnector.MetaDataGruppierung(idGruppe).ConfigureAwait(false);
            var match = meta.actions.FirstOrDefault(stringToCheck => stringToCheck.Contains("CREATE"));
            if (match != null)
            { return true; }
            else
            { return false; }
        }
        public async Task<Boolean> CheckPermissionForEdit(int idGruppe)
        {
            Meta_Data meta = await mainC.mVConnector.MetaDataGruppierung(idGruppe).ConfigureAwait(false);
            var match = meta.actions.FirstOrDefault(stringToCheck => stringToCheck.Contains("UPDATE"));
            if (match != null)
            { return true; }
            else
            { return false; }
        }

    }
}
