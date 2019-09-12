using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Models
{
    public class Device
    {
        public int Id { get; set; }
        public User User { get; set; }
        public string IpAdress { get; set; }
    }
}
