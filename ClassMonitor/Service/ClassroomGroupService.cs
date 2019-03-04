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
    class ClassroomGroupService
    {
        public List<ClassroomGroup> GetClassroomGroups()
        {
            using (PersistenceContext pc = new PersistenceContext())
            {
                ClassroomGroupDao dao = new ClassroomGroupDao(pc);
                List<ClassroomGroup> result = dao.GetClassroomGroups();
                return result;
            }
        }
    }
}
