﻿using ClassMonitor.Dao.framework;
using System.Data.SqlClient;

namespace ClassMonitor.Dao
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