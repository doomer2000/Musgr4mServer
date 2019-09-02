using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Models
{
    public class GeneralChat
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public string Title { get; set; }
        public string ImagePath { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
