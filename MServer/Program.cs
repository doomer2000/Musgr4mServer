using MServer.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer
{
    class Program
    {
        static Server server;
        static void Main(string[] args)
        {
            server = new Server();
            server.start();
            //Started
        }
    }
}
