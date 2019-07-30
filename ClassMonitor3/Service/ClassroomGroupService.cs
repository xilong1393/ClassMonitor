using ClassMonitor3.Dao;
using ClassMonitor3.Dao.framework;
using ClassMonitor3.Model;
using System;
using System.Collections.Generic;

namespace ClassMonitor3.Service
{
    class ClassroomGroupService
    {
        public void CheckSchedule(string sessionID, int classGroupID)
        {
            throw new NotImplementedException();
        }

        public List<ClassroomGroup> GetClassroomGroups(string sessionID)
        {
            using (PersistenceContext pc = new PersistenceContext())
            {
                ClassroomGroupDao classroomGroupDao = new ClassroomGroupDao(pc);
                List<ClassroomGroup> list = classroomGroupDao.GetClassroomGroups();
                return list;
            }
        }

        public List<ClassroomGroup> GetClassroomListByGroupID(string sessionID, int classGroupID)
        {
            throw new NotImplementedException();
        }

        public void GroupSchedule(string sessionID, int classGroupID)
        {
            throw new NotImplementedException();
        }
    }
}
