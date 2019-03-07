using ClassMonitor.Dao;
using ClassMonitor.Dao.framework;
using ClassMonitor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassMonitor.Service
{
    class ClassroomService
    {
        public List<ClassroomView> GetClassroomByGroupID(int GroupID) {
            using (PersistenceContext pc = new PersistenceContext())
            {
                ClassroomDao dao = new ClassroomDao(pc);
                List<ClassroomView> list = dao.GetClassroomByGroupID(GroupID);
                return list;
            }
        }
    }
}
