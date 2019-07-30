﻿using ClassMonitor3.Dao;
using ClassMonitor3.Dao.framework;
using ClassMonitor3.Model;
using System;

namespace ClassMonitor3.Service
{
    public class UserService
    {
        public void LogUserLogin(string sessionID, string employerID, DateTime loginTime, string loginID)
        {
            throw new NotImplementedException();
        }

        public void LogUserOperation(string sessionID, string operation, DateTime operationTime)
        {
            throw new NotImplementedException();
        }

        public User UserLogin(string userName, string password)
        {
            using (PersistenceContext pc = new PersistenceContext())
            {
                UserDao userDao = new UserDao(pc);
                User user = userDao.GetUser(userName, password);
                return user;
            }
        }
    }
}
