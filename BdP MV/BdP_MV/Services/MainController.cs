using BdP_MV.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace BdP_MV.Services
{
    public class MainController
    {
        public MVConnector mVConnector { private set; get; }
        public Group_Control groupControl { private set; get; }
        public Settings einsteillungen { private set; get; }
        public Mitglieder_Control mitgliederController { private set; get; }
        public MainController()
        {
            mVConnector = new MVConnector();
            groupControl = new Group_Control(this);
            einsteillungen = new Settings();
            mitgliederController = new Mitglieder_Control(this);
        }
          
    }
}
