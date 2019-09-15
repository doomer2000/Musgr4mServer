using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Models.TestChat
{
    public class MyChat
    {
        public int Id { get; set; }
        public User User { get; set; }
        public Chat Chat { get; set; }
    }
}
