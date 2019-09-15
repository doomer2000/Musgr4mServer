using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Models.TestChat
{
    public class ChatMember
    {
        public int Id { get; set; }
        public Chat Chat { get; set; }
        public User User { get; set; }
    }
}
