using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Models
{
    public class Friend
    {
        public int Id { get; set; }
        public int? User_Id { get; set; }
        [ForeignKey("User_Id")]
        public User _myUser { get; set; }
        public int? Friend_Id { get; set; }
        [ForeignKey("Friend_Id")]
        public User _Friend { get; set; }
    }
}
