using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMonitor.Model
{
    class ClassroomView
    {
        public int ClassroomID { get; set; }

        public string ClassroomName { get; set; }
        public string EngineStatus { get; set; }

        public string AgentStatus { get; set; }
        public string PPCConnectionStatus { get; set; }

        public string AVStatus { get; set; }
        public string CameraStatus { get; set; }
        public string SGStatus { get; set; }
        public int FreeDisk { get; set; }
    }
}
