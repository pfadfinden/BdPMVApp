using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BdP_MV.View
{

    public class MainMaster_DetailMenuItem
    {
        public MainMaster_DetailMenuItem()
        {
            TargetType = typeof(MainMaster_DetailDetail);
        }
        public int Id { get; set; }
        public string Title { get; set; }

        public Type TargetType { get; set; }
    }
}