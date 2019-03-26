﻿using ClassMonitor.Dao.framework;
using ClassMonitor.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ClassMonitor.Dao
{
     class ClassroomGroupDao:BaseDao
    {
        private const string QUERY_ALL_CLASSROOMGROUP_SQL = "SELECT * FROM CLASSROOMGROUP";
        public ClassroomGroupDao(PersistenceContext pc) : base(pc) { }

        public List<ClassroomGroup> GetClassroomGroups() {
            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = Connection;
                command.CommandText = QUERY_ALL_CLASSROOMGROUP_SQL;
                List<ClassroomGroup> list = SqlHelper.ExecuteReaderCmdList<ClassroomGroup>(command);
                return list;
            }
        }
    }
}
