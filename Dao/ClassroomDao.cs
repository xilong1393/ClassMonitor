using ClassMonitor.Dao.framework;
using ClassMonitor.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMonitor.Dao
{
     class ClassroomDao:BaseDao
    {
        private const string QUERY_CLASSROOMBYID_SQL = "SELECT * FROM CLASSROOM WHERE CLASSROOMGROUPID=@GROUPID";

        private const string QUERY_CLASSROOMBYGROUPID_SQL = "SELECT a.ClassroomID, a.ClassroomName, b.EngineStatus, c.AgentStatus, b.PPCConnectionStatus, " +
            "b.AVStatus, b.CameraStatus, b.SGSTatus, b.FreeDisk FROM CLASSROOM a " +
            "left join ClassroomEngineStatus b on a.ClassroomID=b.ClassroomID " +
            "left join CLassroomAgentStatus c on b.ClassroomID=c.ClassroomID " +
            "WHERE CLASSROOMGROUPID=@GROUPID";
        public ClassroomDao(PersistenceContext pc) : base(pc) { }
        public List<ClassroomView> GetClassroomByGroupID(int GroupID) {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandText = QUERY_CLASSROOMBYGROUPID_SQL;
                command.Parameters.AddWithValue("@GROUPID", GroupID);
                List<ClassroomView> list = SqlHelper.ExecuteReaderCmdList<ClassroomView>(command);
                return list;
            }
        }
    }
}
