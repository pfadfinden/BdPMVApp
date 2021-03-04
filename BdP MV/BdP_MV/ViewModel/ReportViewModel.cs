using BdP_MV.Model;
using BdP_MV.Model.Metamodel;
using BdP_MV.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BdP_MV.ViewModel
{
    class ReportViewModel : BaseNavigationViewModel
    {
        public List<Report_Data> reportdata;
        MainController mainc;
        public Gruppe aktGruppe;
        ReportViewModel()
        {
            mainc = new MainController();

        }
        public async Task GetReportsByGroup(int groupId)
        {
            reportdata = await mainc.mVConnector.ReportData(groupId);

        }

    }
}
