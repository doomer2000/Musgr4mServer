using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Models
{
    public class GeneralChatUsers
    {
        public GeneralChat GeneralChatId { get; set; }
        public User UserId { get; set; }
        public bool IsShow { get; set; }
    }
}
