using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MServer.Models;

namespace MServer.Repositoies.Interfaces
{
    public interface IUserService
    {
        Task<User> IsUserExist(string login, string password);
        Task<User> TryLogin(string login, string password);
       
        void SetOnline(int id);

    }
}
