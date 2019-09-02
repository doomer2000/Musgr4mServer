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
        User GetUser(string login, string password);
    }
}
