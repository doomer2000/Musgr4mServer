using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MServer.Models.TestChat
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int MessageType { get; set; }
        public DateTime SendTime { get; set; }
        //public DateTime ReceiveTime { get; set; }
        //public DateTime ReadTime { get; set; }
        //Later
        public string ImagePath { get; set; }
        public string VideoPath { get; set; }
        public string MusicPath { get; set; }
        public string VoicePath { get; set; }


        public virtual User User { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
