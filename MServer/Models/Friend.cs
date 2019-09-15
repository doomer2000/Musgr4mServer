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
        public User User { get; set; }

        [ForeignKey("_Friend")]
        public int? _Friend_Id { get; set; }
        public User _Friend { get; set; }
    }
}
//use WW_Database;

//INSERT INTO Users(Login, Password, IsOnline, LastTimeOnline)
//VALUES
//(1,1,0, SYSDATETIME()),
//(2,2,0,SYSDATETIME()),
//(3,3,0,SYSDATETIME()),
//(4,4,0,SYSDATETIME());

//INSERT INTO Friends(User_Id, Friend_Id, User_Id1)
//VALUES(1,2,2),
//(1,3,3);


//SELECT* FROM Friends;