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
    public class UserManipulationService : IUserManipulationService
    {
        private Wheel_Context context;

        public UserManipulationService(Wheel_Context context)
        {
            this.context = context;
        }

        public bool ChangePassword(User user, string outPassword, string newPassword)
        {

            user = context.Users.Where(u => user.Id == u.Id).FirstOrDefault();
            if (user.Password == outPassword)
            {
                user.Password = newPassword;
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public async Task<User> GetUserByID(int id)
        {
            User user = await context.Users.FindAsync(id);
            if (user != null)
            {
                user.Password += string.Empty;
                user.MobileNum = string.Empty;
                return user;
            }
            return null;
        }

        public User Registration(User user)
        {
            user = context.Users.Add(user);
            context.SaveChanges();
            return user;
        }
    }
}
