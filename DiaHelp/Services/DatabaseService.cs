using DiaHelp.Infrastructure;
using DiaHelp.Interface;
using DiaHelp.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiaHelp.Services
{
    public class DatabaseService(ApplicationContext context) : IDatabaseService
    {
        public List<User> GetAllUser() => context.Users.ToList();
        public User GetUser(string username) => context.Users.FirstOrDefault(p => p.Username == username);
        public bool AddUser(User userModel)
        {
            try
            {
                context.Users.Add(userModel);
                context.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
