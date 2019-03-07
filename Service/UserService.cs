using ClassMonitor.Dao;
using ClassMonitor.Dao.framework;
using ClassMonitor.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace ClassMonitor.Service
{
    class UserService
    {
        public bool UserExists(string Name,string Password) {
            using (PersistenceContext pc = new PersistenceContext()) {
                UserDao dao = new UserDao(pc);
                bool result = dao.UserExists(Name, Password);
                return result;
            }
        }

        public bool UserLogin(string Name, string Password)
        {
            using (PersistenceContext pc = new PersistenceContext())
            {
                UserDao dao = new UserDao(pc);
                bool result = dao.UserExists(Name, Password);
                return result;
            }
        }

        public User GetUser(string Name, string Password) {
            using (PersistenceContext pc = new PersistenceContext())
            {
                UserDao dao = new UserDao(pc);
                User user = dao.GetUser(Name, Password);
                return user;
            }
        }
    }
}
