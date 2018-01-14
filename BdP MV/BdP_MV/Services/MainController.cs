using System;
using System.Collections.Generic;
using System.Text;

namespace BdP_MV.Services
{
    public class MainController
    {
        public MVConnector mVConnector { private set; get; }
        public MainController()
        {
            mVConnector = new MVConnector();
        }
          
    }
}
