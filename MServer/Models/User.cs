using System;
using System.Collections.Generic;
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
        public DateTime LastTimeOnline { get; set; }
        public string SecretAnswer { get; set; }
        public string AvatarPath { get; set; }
        
        public virtual SecretQuestion SecretQuestion { get; set; }
        public virtual ICollection<Music> UserMusic { get; set; }
        public virtual ICollection<Device> Devices { get; set; }
        public virtual ICollection<GeneralChat> GeneralChats { get; set; }
        public virtual ICollection<User> UserFriends { get; set; }
    }
}
