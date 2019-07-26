using ClassMonitor3.Dao;
using ClassMonitor3.Dao.framework;
using ClassMonitor3.Model;
using System.Transactions;

namespace ClassMonitor3.Service
{
    public class LogService
    {
        public bool LogUserLogin(UserLoginLog userLoginLog)
        {
            using (PersistenceContext pc = new PersistenceContext(IsolationLevel.ReadCommitted)) {
                LogDao dao = new LogDao(pc);
                bool result = dao.InsertUserLogin(userLoginLog);
                if (result) pc.Commit();
                return result;
            }
        }

        public bool LogUserOperation(UserOperationLog userOperationLog)
        {
            using (PersistenceContext pc = new PersistenceContext(IsolationLevel.ReadCommitted))
            {
                LogDao dao = new LogDao(pc);
                bool result = dao.InsertUserOperation(userOperationLog);
                if (result) pc.Commit();
                return result;
            }
        }
    }
}
