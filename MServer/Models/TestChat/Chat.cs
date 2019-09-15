using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Models.TestChat
{
    public class Chat
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsPrivate { get; set; }
        public string ImagePath { get; set; }
        public virtual ICollection<ChatMember> ChatMembers{ get; set; }
    }
}
