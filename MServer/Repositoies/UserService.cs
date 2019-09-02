using MServer.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MServer.Models;
using MServer.Repositoies.Interfaces;

namespace MServer.Repositoies
{
    public class UserService : IUserService
    {
        private Wheel_Context context;

        public UserService(Wheel_Context context)
        {
            this.context = context;
        }

        public User GetUser(string login, string password)
        {
            User user = context.Users.Where(u => login == u.Login && password == u.Password).FirstOrDefault();
            if (user != null)
            {
                return user;
            }
            return null;
        }


    }
}
