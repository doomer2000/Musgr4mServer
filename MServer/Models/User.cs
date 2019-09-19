using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Mail { get; set; }
        public string MobileNum { get; set; }
        public bool IsOnline { get; set; }
        public string LastTimeOnline { get; set; }
     
        public string AvatarPath { get; set; }
        
       
        public virtual ICollection<Music> UserMusic { get; set; }

        [InverseProperty("_Friend")]
        public virtual ICollection<Friend> UserFriends { get; set; }
    }
}
