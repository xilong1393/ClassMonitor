using ClassMonitor3.Dao.framework;
using System.Data.SqlClient;

namespace ClassMonitor3.Dao
{
    class BaseDao
    {
        private PersistenceContext persistenceContext;

        protected SqlConnection Connection
        {
            get
            {
                return persistenceContext.Connection;
            }
        }
        protected BaseDao(PersistenceContext persistenceContext)
        {
            this.persistenceContext = persistenceContext;
        }
    }
}
