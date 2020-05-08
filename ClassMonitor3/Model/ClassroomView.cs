using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMonitor3.Model
{
    public class ClassroomView
    {
        [Browsable(false)]
        public int ClassroomID { get; set; }
        public string ClassroomName { get; set; }
        [Browsable(false)]
        public string PPCPublicIP { get; set; }
        [Browsable(false)]
        public int PPCPort { get; set; }
        public string EngineStatus { get; set; }
        public string AgentStatus { get; set; }
        [DisplayName("PPC")]
        public string PPCConnectionStatus { get; set; }
        //course
        public string AVStatus { get; set; }
        public string WBNumber { get; set; }
        public string KaptivoNumber { get; set; }
        public string Status { get; set; }
        //SD
        public int FreeDisk { get; set; }
    }
}
