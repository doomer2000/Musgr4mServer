using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MServer.Models;

namespace MServer.Repositoies.Interfaces
{
    public interface IUserManipulationService
    {
        User Registration(User user);
        bool ChangePassword(User user, string outPassword, string newPassword);
        Task<User> GetUserByID(int id);
    }
}
