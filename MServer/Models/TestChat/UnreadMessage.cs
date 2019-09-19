using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Models.TestChat
{
    public class UnreadMessage
    {
        public int Id { get; set; }
        public Chat Chat { get; set; }

        public User UserTW { get; set; }
        
        public Message Message { get; set; }
    }
}
