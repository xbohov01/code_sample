using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Source.ManualFlight
{ 
    public class EngineStatus
    {
        public int EngineId;
        public string EngineStatusString;
        public string EngineName;

        public EngineStatus(int id, string name)
        {
            this.EngineName = name;
            this.EngineId = id;
            this.EngineStatusString = "off";
        }
    }
}
