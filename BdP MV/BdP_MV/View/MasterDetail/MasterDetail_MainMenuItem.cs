using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.View.MasterDetail
{

    public class MasterDetail_MainMenuItem
    {
        public MasterDetail_MainMenuItem()
        {
            TargetType = typeof(MasterDetail_MainDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}